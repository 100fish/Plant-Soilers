using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

public class FrameDifference : MonoBehaviour
{
    public Material differenceMaterial;
    public Camera cam;

    private RenderTexture previousFrame;
    private RenderTexture currentFrame;


    //public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameContext)
    //{
    //    UniversalCameraData cameraData = frameContext.Get<UniversalCameraData>();
    //}








    //Runs after the final frame's image is rendered
    //Declares two new RenderTextures
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //Debug.Log("helloooo");

        //if (prevFrame == null)
        //{
            //we create the RenderTexture that displays our final image
            //This code only runs once !
        //    prevFrame = new RenderTexture(source.width, source.height, 0);
        //}

        // Set textures in shader
        //differenceMaterial.SetTexture("_MainTexture", source);
        //differenceMaterial.SetTexture("_LastFrameTexture", prevFrame);

        // Blit current scene to processed output
        // 
        //Graphics.Blit(source, destination, differenceMaterial);

        // Copy this frame's rendered image to the PREVFRAME rendertexture,
        // Storing it
        //Graphics.Blit(source, prevFrame);
    }
}
