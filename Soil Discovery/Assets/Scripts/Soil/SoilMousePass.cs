using Unity.VisualScripting;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine;
using UnityEngine.InputSystem;

public class SoilMousePass : MonoBehaviour
{
    [SerializeField] private Material soilMaterial;
    [SerializeField] private Material visibleMaterial;
    [SerializeField] private TouchInput touchInput;


    private SoilInput playerControls;

    [SerializeField] private float digTimerMax;
    private float digTimer;

    private Vector3 mousePositionOnObject;
    private Vector2 lastPosition = new(0,0);

    private void OnEnable()
    {
        playerControls.Enable(); //Enabling and disabling the Input Asset is required 
    }

    private void OnDisable()
    {
        playerControls.Disable(); //Enabling and disabling the Input Asset is required

        soilTex.Release();
    }

    private void Awake()
    {
        playerControls = new SoilInput();
    }

    //[SerializeField] private Texture startTexture;

    [SerializeField] private bool UseTouchSphereInsteadOfRaycast = true;

    private Vector2 mousePosition;
    private RenderTexture soilTex;

    //Stores the ID of the Shader property this script updates
    int mousePositionField;

    void Start()
    {

       

        //Since we update this shader property ever frame its faster to get the ID
        mousePositionField = Shader.PropertyToID("_MousePosition");

        //creates a new rendertexture that we use as the texture for the object 
        soilTex = new RenderTexture(new RenderTextureDescriptor(1024, 1024));

        //sets the texture in the shader to this texture
        soilMaterial.SetTexture("_MainTex", soilTex);
        visibleMaterial.SetTexture("_SoilMap", soilTex);


    }

    // Update is called once per frame
    void Update()
    {
        //Get the mouse's position in Screenspace
        mousePosition = Mouse.current.position.ReadValue();

        if (/*playerControls.Testing.Dig.inProgress && */ digTimer <= 0)
        {

            if (UseTouchSphereInsteadOfRaycast == false)
            {
                //Debug.Log("Using raycast to find mouse position");
                //set position to ball's position
                Vector2 touchPosition = TouchInput.Instance.hitMesh.textureCoord;
                //Debug.Log("Mouse position on object: " + touchPosition);
                //Set the shader's mouseposition value to the mouse's position in screen space
                SetMousePosition(touchPosition);
                
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

            digTimer = digTimerMax;
        }
        else
        {
            digTimer -= Time.deltaTime;
        }
    }

    private void SetMousePosition(Vector2 mouseCoord)
    {
        if(mouseCoord != lastPosition)
        {
            soilMaterial.SetVector(mousePositionField, mouseCoord);

            RenderTexture temp = RenderTexture.GetTemporary(1024, 1024);
            Graphics.Blit(soilTex, temp, soilMaterial);
            Graphics.Blit(temp, soilTex);

            RenderTexture.ReleaseTemporary(temp);
        }

        lastPosition = mouseCoord;
    }

    

}
