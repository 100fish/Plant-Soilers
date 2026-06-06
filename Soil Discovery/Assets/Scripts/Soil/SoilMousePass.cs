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

    Texture2D fullTex;

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
        fullTex = new Texture2D(1024, 1024);

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


        /*for (int i = 0; i < currentTouchValues.Length; i++)
        {
            Color tempColor = GetPixelFromRT(temp, Mathf.RoundToInt((int)touchPositions[i].x), Mathf.RoundToInt((int)touchPositions[i].y));
            Debug.Log($"{i}th Position at: x{Mathf.RoundToInt((int)touchPositions[i].x)} y{Mathf.RoundToInt((int)touchPositions[i].y)} is {tempColor.b}");

            //currentTouchValues[i] = tempColor.r;
        }*/

        RenderTexture.ReleaseTemporary(temp);
    }

    public Color GetPixelFromRT(RenderTexture rt, int x, int y)
    {
        RenderTexture.active = rt;

        //Texture2D.ReadPixels(new Rect(0, 0, rt.width, rt.height),0, 0);

        fullTex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        fullTex.Apply();

        Color pixelColor = fullTex.GetPixel(x, y);

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
