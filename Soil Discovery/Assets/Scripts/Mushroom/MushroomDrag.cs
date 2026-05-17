using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class MushroomDrag : MonoBehaviour
{
    private bool isDragging = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isDragging == true)
        {
            int thisIndex = TouchInput.Instance.touchBalls.IndexOf(gameObject);
            Debug.Log("This index: " + thisIndex);
            int thisTouchID = TouchInput.Instance.touchesList[thisIndex];
            Debug.Log("This touch ID: " + thisTouchID);
            UnityEngine.InputSystem.EnhancedTouch.Touch thisTouch = UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches[thisTouchID];
            Debug.Log("This touch phase: " + thisTouch.phase);
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
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Started trigger mushroom.");
        if (other.CompareTag("SHROOM"))
        {
            Debug.Log("Started dragging mushroom.");
            isDragging = true;
        }
    }
}

