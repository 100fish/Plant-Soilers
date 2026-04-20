using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.RenderGraphModule;
public class RenderLastFrameInMaterial : ScriptableRendererFeature
{
    public Material objectMaterial;
    CustomRenderPass renderLastFrame;


    public override void Create()
    {
        renderLastFrame = new CustomRenderPass();
        renderLastFrame.renderPassEvent = RenderPassEvent.BeforeRenderingOpaques;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderLastFrame.passMaterial = objectMaterial;
        renderer.EnqueuePass(renderLastFrame);
    }

    class CustomRenderPass : ScriptableRenderPass
    {
        public Material passMaterial;

        class PassData
        {
            public Material material;
            public TextureHandle historyTexture;
            public TextureHandle currentTexture; //Griff line

        }

        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer contextData)
        {
            UniversalCameraData cameraData = contextData.Get<UniversalCameraData>();

            // Return if the history manager isn't available
            // For example, there are no history textures during the first frame
            if (cameraData.historyManager == null) { return; }

            // Request access to the color and depth textures
            cameraData.historyManager.RequestAccess<RawColorHistory>();

            using (var builder = renderGraph.AddRasterRenderPass<PassData>("Get last frame", out var passData))
            {
                UniversalResourceData resourceData = contextData.Get<UniversalResourceData>();

                // Set the render graph to render to the active color texture
                builder.SetRenderAttachment(resourceData.activeColorTexture, 0, AccessFlags.Write);

                // Add the material to the pass data
                passData.material = passMaterial;

                // Get the color texture the camera rendered to in the previous frame
                RawColorHistory history = cameraData.historyManager.GetHistoryForRead<RawColorHistory>();
                RTHandle currentTexture = history?.GetCurrentTexture(0); //Griff line
                RTHandle historyTexture = history?.GetPreviousTexture(0);
                passData.historyTexture = renderGraph.ImportTexture(historyTexture);
                passData.currentTexture = renderGraph.ImportTexture(currentTexture); //Griff line



                builder.SetRenderFunc(static (PassData data, RasterGraphContext context) =>
                {
                    // Set the material to use the texture
                    data.material.SetTexture("_BaseMap", data.historyTexture);
                    data.material.SetTexture("_CurrentBaseMap", data.currentTexture); //Griff line
                });
            }
        }
    }
}