using Unity.VisualScripting;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

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
    private int[] bugPositionFields = new int[6];

    private float[] currentTouchValues = new float[10];

    private Vector2[] touchPositions = new Vector2[10];

    private Vector2[] previousTouchPositions = new Vector2[10];
    private Vector2[] previousBugPositions = new Vector2[10];


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

        if (playerControls.Testing.Restart.triggered)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (/*playerControls.Testing.Dig.inProgress &&*/ digTimer <= 0)
        {
            //set position to ball's position
            touchPositions[0] = touchInput.hitMesh.textureCoord;


            //touchPositions[0] = Mouse.current.position;

            //Set the shader's mouseposition value to the mouse's position in screen space
            Dig(touchPositions);
                
                //SetMousePosition(mousePositionOnObject);


            digTimer = digTimerMax;
        }
        else
        {
            digTimer -= Time.deltaTime;
        }
    }

    //will be used once multitouch is set up
    private void Dig(Vector2[] touchPositions)
    {
        //check if any touchs are the same
        /*for (int i = 0; i < touchPositions.Length; i++)
        {
            for (int ii = 0; ii < previousTouchPositions.Length; ii++)
            {
                if (touchPositions[i] == previousTouchPositions[ii])
                {
                    touchPositions[i] = new Vector2(0, 0);
                }
            }
        } */

        //Set the touch positions in the shader and store them for the next frame's comparison
        for (int i = 0; i < touchPositions.Length; i++)
        {

            if (null == previousTouchPositions[i] || touchPositions[i] != previousTouchPositions[i])
            {
                soilMaterial.SetVector(touchPositionFields[i], touchPositions[i]);
            }
            else
            {
                soilMaterial.SetVector(touchPositionFields[i], Vector4.zero);
            }

            previousTouchPositions[i] = touchPositions[i];
        }

        Vector2[] bugPositions = BugManager.Instance.GetBugPositionArray();
        //Set the bug positions in the shader and store them for the next frame's comparison

        //Debug.Log(bugPositions.Length);
        //Debug.Log(bugPositionFields.Length);

        for (int i = 0; i < bugPositions.Length; i++)
        {

            if (null == previousBugPositions[i] || bugPositions[i] != previousBugPositions[i])
            {
                soilMaterial.SetVector(bugPositionFields[i], bugPositions[i]);
            }
            else
            {
                soilMaterial.SetVector(bugPositionFields[i], Vector2.zero);
            }

            previousBugPositions[i] = bugPositions[i];
        }

        //actually draw
        RenderTexture temp = RenderTexture.GetTemporary(1024, 1024);
        Graphics.Blit(soilTex, temp, soilMaterial);
        Graphics.Blit(temp, soilTex);


        for (int i = 0; i < currentTouchValues.Length; i++)
        {
            Color tempColor = GetPixelFromRT(temp, Mathf.RoundToInt(touchPositions[i].x), Mathf.RoundToInt(touchPositions[i].y));

            currentTouchValues[i] = tempColor.r;
        }

        RenderTexture.ReleaseTemporary(temp);
    }

    public Color GetPixelFromRT(RenderTexture rt, int x, int y)
    {
        // Remember current active RT to restore later
        RenderTexture currentActiveRT = RenderTexture.active;

        // Set target RT as active
        RenderTexture.active = rt;

        // Create a 1x1 Texture2D to hold just the single pixel (saves memory)
        Texture2D tex = new Texture2D(1, 1, TextureFormat.RGB24, false);

        // Read the specific pixel at (x, y) into the 1x1 texture
        tex.ReadPixels(new Rect(x, y, 1, 1), 0, 0);
        tex.Apply();

        // Restore the previous active RT
        RenderTexture.active = currentActiveRT;

        Color pixelColor = tex.GetPixel(0, 0);
        Destroy(tex);

        return pixelColor;
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

            for (int i = 0; i < currentTouchValues.Length; i++)
            {
                Color tempColor = GetPixelFromRT(temp, Mathf.RoundToInt(mouseCoord.x), Mathf.RoundToInt(mouseCoord.y));

                currentTouchValues[i] = tempColor.r;
            }

            RenderTexture.ReleaseTemporary(temp);
        }

        lastPosition = mouseCoord;
    }

    

}
