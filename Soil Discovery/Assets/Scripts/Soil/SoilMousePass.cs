using Unity.VisualScripting;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class SoilMousePass : MonoBehaviour
{
    [SerializeField] private Material soilMaterial;
    [SerializeField] private Material visibleMaterial;
    [SerializeField] private TouchInput touchInput;


    private SoilInput playerControls;

    [SerializeField] private float digTimerMax;
    private float digTimer;


    private Vector2 mousePositionOnObject;
    private Vector2 lastPosition = new(0,0);

    private Vector2 mousePosition;
    private RenderTexture soilTex;

    //Stores the IDs of the Shader property this script updates
    private int[] touchPositionFields = new int[10];
    private int[] bugPositionFields = new int[10];

    private Vector2[] previousTouchPositions = new Vector2[10];

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

    void Start()
    {
        //Since we update this shader property ever frame its faster to get the ID
        for (int i = 0; i < touchPositionFields.Length; i++)
        {
            touchPositionFields[i] = Shader.PropertyToID($"_TouchPos{i+1}");
            Debug.Log(touchPositionFields[i]);
        }

        for (int i = 0; i < bugPositionFields.Length; i++)
        {
            bugPositionFields[i] = Shader.PropertyToID($"_BugPos{i + 1}");
            Debug.Log(bugPositionFields[i]);
        }

        //creates a new rendertexture that we use as the texture for the object 
        soilTex = new RenderTexture(new RenderTextureDescriptor(1024, 1024));

        //sets the texture in the shader to this texture
        soilMaterial.SetTexture("_MainTex", soilTex);
        visibleMaterial.SetTexture("_SoilMap", soilTex);


    }

    // Update is called once per frame
    void Update()
    {

        if (/*playerControls.Testing.Dig.inProgress &&*/ digTimer <= 0)
        {
                //set position to ball's position
                mousePositionOnObject = touchInput.hitMesh.textureCoord;
                

                //Set the shader's mouseposition value to the mouse's position in screen space
                SetMousePosition(mousePositionOnObject);


            digTimer = digTimerMax;
        }
        else
        {
            digTimer -= Time.deltaTime;
        }
    }

    //will be used once multitouch is set up
    public void Dig(Vector2[] touchPositions, Vector2[] bugPositions)
    {
        //check if any touchs are the same
        for (int i = 0; i < touchPositions.Length; i++)
        {
            for (int ii = 0; ii < previousTouchPositions.Length; ii++)
            {
                if (touchPositions[i] == previousTouchPositions[ii])
                {
                    touchPositions[i] = new Vector2(-10000, -10000);
                }
            }
        }

        //Set the touch positions in the shader and store them for the next frame's comparison
        for (int i = 0; i < touchPositions.Length; i++)
        {
            soilMaterial.SetVector(touchPositionFields[i], touchPositions[i]);

            previousTouchPositions[i] = touchPositions[i];
        }

        //implement bug digging later

        //actually draw
        RenderTexture temp = RenderTexture.GetTemporary(1024, 1024);
        Graphics.Blit(soilTex, temp, soilMaterial);
        Graphics.Blit(temp, soilTex);

        RenderTexture.ReleaseTemporary(temp);
    }

    //will become defunct once multitouch is set up
    private void SetMousePosition(Vector2 mouseCoord)
    {
        if(mouseCoord != lastPosition)
        {
            soilMaterial.SetVector(touchPositionFields[0], mouseCoord);

            RenderTexture temp = RenderTexture.GetTemporary(1024, 1024);
            Graphics.Blit(soilTex, temp, soilMaterial);
            Graphics.Blit(temp, soilTex);

            RenderTexture.ReleaseTemporary(temp);
        }

        lastPosition = mouseCoord;
    }

    

}
