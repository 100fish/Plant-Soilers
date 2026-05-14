using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
public class NewMonoBehaviourScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EnhancedTouchSupport.Enable();
    }

    public GameObject objectToSpawn;
    public Camera mainCamera;
    public GameObject TouchBall;
    // Update is called once per frame
    void Update()
    {
        foreach (UnityEngine.InputSystem.EnhancedTouch.Touch touches in UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches)
        {
            if (touches.phase == UnityEngine.InputSystem.TouchPhase.Began || touches.phase == UnityEngine.InputSystem.TouchPhase.Moved)
            {
                Vector2 touchPosition = touches.screenPosition;
                RaycastHit hit;
                if (Physics.Raycast(mainCamera.ScreenPointToRay(touchPosition), out hit))
                {
                    Vector3 touchSpotZFix = new Vector3(hit.point.x, hit.point.y, 8); //Arbitrary z value to keep the object visible
                    TouchBall.transform.position = touchSpotZFix;
                }
            }
            if(touches.phase == UnityEngine.InputSystem.TouchPhase.Ended || touches.phase == UnityEngine.InputSystem.TouchPhase.None)
            {
                TouchBall.transform.position = new Vector3(0, -100, 0);
            }
        }
    }
}
