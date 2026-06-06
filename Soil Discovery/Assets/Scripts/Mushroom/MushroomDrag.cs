using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class MushroomDrag : MonoBehaviour
{
    public List<GameObject> emptyMushroomPrefabs;
    private bool isDragging = false;
    UnityEngine.InputSystem.EnhancedTouch.Touch thisTouch;
    Vector2 lastDrop = Vector2.zero;
    Vector2 currentDrop = Vector2.zero;


    /*
    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        TouchSimulation.Enable();
    }

    private void OnDisable()
    {
        TouchSimulation.Disable();
    } */

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //EnhancedTouchSupport.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDragging == true)
        {
            int thisIndex = TouchInput.Instance.touchBalls.IndexOf(gameObject);
            //Debug.Log("This index: " + thisIndex);
            int thisTouchID = TouchInput.Instance.touchesList[thisIndex];
           // Debug.Log("This touch ID: " + thisTouchID);
            foreach (UnityEngine.InputSystem.EnhancedTouch.Touch activeTouchVar in UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches)
            {
                //Debug.Log("Active touch ID: " + activeTouchVar.touchId);
                if (activeTouchVar.touchId == thisTouchID)
                {
                    thisTouch = activeTouchVar;
                }
            }
           // Debug.Log("pig " + thisTouch.touchId);
            //Debug.Log("This touch phase: " + thisTouch.phase);
            if (thisTouch.phase == UnityEngine.InputSystem.TouchPhase.Ended)
            {
                Debug.Log("Stopped dragging mushroom.");
                isDragging = false;
            }   
            if(thisTouch.phase == UnityEngine.InputSystem.TouchPhase.None)
            {
                Debug.Log("I dont exist bruh");
                isDragging = false;
            }
            if (thisTouch.phase == UnityEngine.InputSystem.TouchPhase.Moved)
            {
                Debug.Log("Dragging mushroom...");
                currentDrop = transform.position;

                if(lastDrop == Vector2.zero)
                {
                    lastDrop = transform.position;
                }

                if ((lastDrop - currentDrop).magnitude > 0.3f)
                {
                    lastDrop = currentDrop;
                    Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y, 7.9f); //Arbitrary z value to keep the object visible
                    Instantiate(emptyMushroomPrefabs[Random.Range(0, 5)], spawnPos, Quaternion.identity); //hardcoded to 5 casue count not working for some reason, but we have 5 mushroom prefabs
                }

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Started trigger mushroom.");
        if (other.CompareTag("SHROOM"))
        {
            //Debug.Log("Started dragging mushroom.");
            isDragging = true;
        }
    }
}

