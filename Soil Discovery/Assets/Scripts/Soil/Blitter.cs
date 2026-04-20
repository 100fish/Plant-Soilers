using UnityEngine;

public class Blitter : MonoBehaviour
{
    [SerializeField] private RenderTexture soilRenderTexture;
    private RenderTexture oldRenderTexture;

    [SerializeField] private Material soilMaterial;
    [SerializeField] private Texture soilTexture;



    void Start()
    {
        oldRenderTexture = new RenderTexture(soilRenderTexture);
     }

    // Update is called once per frame
    void Update()
    {

        //soilMaterial.SetTexture("_RenderTexture", soilRenderTexture);

        Graphics.Blit(soilMaterial.GetTexture("_Texture"), soilRenderTexture);



        oldRenderTexture = soilRenderTexture;

    }
}
