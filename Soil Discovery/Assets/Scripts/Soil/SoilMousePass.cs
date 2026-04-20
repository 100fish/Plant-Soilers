using UnityEngine;
using UnityEngine.InputSystem;

public class SoilMousePass : MonoBehaviour
{
    [SerializeField] private Material soilMaterial;

    [SerializeField] private bool UseObjectLocalSpaceInsteadOfScreenSpace = true;

    Vector2 mousePosition;
    Vector2 oldMousePosition;

    //Stores the ID of the Shader property this script updates
    int mousePositionField;
    int oldMousePositionField;

    void Start()
    {
        //Since we update this shader property ever frame its faster to get the ID
        mousePositionField = Shader.PropertyToID("_MousePosition");
        oldMousePositionField = Shader.PropertyToID("_OldMousePosition");
    }

    // Update is called once per frame
    void Update()
    {
        //Get the mouse's position in Screenspace
        mousePosition = Mouse.current.position.ReadValue();
        Debug.Log("MousePosition: " + mousePosition);

        if(UseObjectLocalSpaceInsteadOfScreenSpace == false)
        {
            //adjust the mouse position to be on a scale of zero to one on each axis
            mousePosition.x /= Screen.width;
            mousePosition.y /= Screen.height;

            //Set the shader's mouseposition value to the mouse's position in screen space
            SetMousePosition(mousePosition);
        }
        else
        {
            //Send a ray to find the mouse's position in local space
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                //store as Vector3 Just in case with need the z?
                Vector3 mousePositionOnObject = hit.textureCoord;
                SetMousePosition(mousePositionOnObject);
            }
        }
    }

    private void SetMousePosition(Vector2 mouseCoord)
    {
        soilMaterial.SetVector(mousePositionField, mouseCoord);
        Debug.Log("Current coord: " + mouseCoord);
        soilMaterial.SetVector(oldMousePositionField, oldMousePosition);
        Debug.Log("Old coord: " + oldMousePosition);


        //Set new omp for next frame
        oldMousePosition = mouseCoord;

    }
}
