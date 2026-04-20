using UnityEngine;
using UnityEngine.InputSystem;

public class SoilMousePass : MonoBehaviour
{
    [SerializeField] private Material soilMaterial;

    int mousePositionField;

    Vector3 mousePosition;
    Vector3 mousePositionOnObject;

    public RenderTexture soilTex;


    void Start()
    {
        mousePositionField = Shader.PropertyToID("_MousePosition");
        //soilTex = new RenderTexture(1024, 1014, 0);
        soilMaterial.SetTexture("_MainTex", soilTex);
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

        RenderTexture temp = RenderTexture.GetTemporary(1024, 1024);
        RenderTexture temp2 = RenderTexture.GetTemporary(1024, 1024);
        Graphics.Blit(soilTex, temp, soilMaterial);
        Graphics.Blit(temp, soilTex);

    }
}
