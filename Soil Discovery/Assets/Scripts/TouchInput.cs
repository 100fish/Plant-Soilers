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
    // Update is called once per frame
    void Update()
    {
        foreach (UnityEngine.InputSystem.EnhancedTouch.Touch touches in UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches)
        {
            if (touches.phase == UnityEngine.InputSystem.TouchPhase.Began)
            {
                Vector2 touchPosition = touches.screenPosition;
                RaycastHit hit;
                if (Physics.Raycast(mainCamera.ScreenPointToRay(touchPosition), out hit))
                {
                    Debug.Log("Touch hit: " + hit.collider.gameObject.name);
                    Instantiate(objectToSpawn, hit.point, Quaternion.identity);
                }
            }
        }
    }
}
