using UnityEngine;
using UnityEngine.InputSystem;

public class SoilMousePass : MonoBehaviour
{
    [SerializeField] private Material soilMaterial;

    int mousePositionField;

    Vector3 mousePosition;
    Vector3 mousePositionOnObject;


    void Start()
    {
        mousePositionField = Shader.PropertyToID("_MousePosition");
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Debug.Log("MousePosition: " + mousePosition);

        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            mousePositionOnObject = hit.textureCoord;
        }


        soilMaterial.SetVector(mousePositionField, mousePositionOnObject);
    }
}
