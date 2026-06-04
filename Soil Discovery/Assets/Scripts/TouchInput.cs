using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;
public class TouchInput : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

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

    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        EnhancedTouchSupport.Enable();

    }

    public static TouchInput Instance { get; private set; }
    public GameObject objectToSpawn;
    public Camera mainCamera;
    public List<GameObject> touchBalls;
    public UnityEngine.InputSystem.EnhancedTouch.Touch[] touches = new UnityEngine.InputSystem.EnhancedTouch.Touch[5];
    public int[] touchesList { get; private set; } = new int[5];
    public RaycastHit hitMesh { get; private set; } //Griff code (removed for now)

    //public RaycastHit hit; //Griff code (removed for now)
    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches.Count; i++)
        {
            //Debug.Log("Touch count: " + UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches.Count);

            UnityEngine.InputSystem.EnhancedTouch.Touch currentTouch = default;
            //Debug.Log("1");
            foreach (UnityEngine.InputSystem.EnhancedTouch.Touch activeTouchVar in UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches)
            {
                //Debug.Log("2");
                if (!touches.Contains(activeTouchVar))
                {
                    //Debug.Log("3");
                    touches[i] = activeTouchVar;
                }
            }
           // Debug.Log("4");
            currentTouch = touches[i];
            //Debug.Log("touchID" + currentTouch.touchId);
            touchesList[i] = currentTouch.touchId;
            //Debug.Log("6");
            GameObject TouchBall = touchBalls[i];
            Debug.Log("TB Start Pos: " + TouchBall.transform.position);

            //Debug.Log("7");
            //Debug.Log(currentTouch);

            //Working below
            if (currentTouch.phase == UnityEngine.InputSystem.TouchPhase.Began || currentTouch.phase == UnityEngine.InputSystem.TouchPhase.Moved)
            {
                //Debug.Log("Touch position: " + currentTouch.screenPosition);
                Vector2 touchPosition = currentTouch.screenPosition;
                RaycastHit hit; //Griff code (removed for now)
                if (Physics.Raycast(mainCamera.ScreenPointToRay(touchPosition), out hit))
                {
                    //Debug.Log("Hit object: " + hit.collider.gameObject.name);
                    hitMesh = hit;
                    Vector3 touchSpotZFix = new Vector3(hit.point.x, hit.point.y, 8); //Arbitrary z value to keep the object visible

                    TouchBall.transform.position = touchSpotZFix;

                    Debug.Log("TB Y Scale = " + TouchBall.transform.localScale.y);

                    Debug.Log("TB Mid Pos: " + TouchBall.transform.position);

                    Debug.Log("TSZF: " + touchSpotZFix);
                }
            }
            if(currentTouch.phase == UnityEngine.InputSystem.TouchPhase.Ended || currentTouch.phase == UnityEngine.InputSystem.TouchPhase.None)
            {
                //Debug.Log("Touch ended or none.");  
                TouchBall.transform.position = new Vector3(0, -100, 0);
            }

            //Clean Up touch list
            foreach (UnityEngine.InputSystem.EnhancedTouch.Touch touchesInList in touches)
            {
                if (UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches.Contains(touchesInList))
                {
                    return;
                }
                else
                {
                    for(int q = 0; q < touches.Length; q++)
                    {
                        if (touches[q].touchId == touchesInList.touchId)
                        {
                            touches[q] = default;
                            for(int r = 0; r < touchesList.Length; r++)
                            {
                                if (touchesList[r] == touchesInList.touchId)
                                {
                                    touchesList[r] = -1; // or any default value indicating no touch
                                }
                            }
                        }
                    }
                }
            }
            Debug.Log("TB End Pos: " + TouchBall.transform.position);

        }
    }
}
