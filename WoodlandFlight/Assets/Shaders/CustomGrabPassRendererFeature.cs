using System;
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.RenderGraphModule;

public class CustomGrabPassRendererFeature : ScriptableRendererFeature
{
    [SerializeField] private GrabSettings settings;
    private GrabRenderPass grabRenderPass;

    public override void Create()
    {
        grabRenderPass = new GrabRenderPass(settings);
        grabRenderPass.renderPassEvent = RenderPassEvent.BeforeRenderingTransparents;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer,
        ref RenderingData renderingData)
    {
        if (grabRenderPass == null)
        {
            return;
        }
        renderer.EnqueuePass(grabRenderPass);
    }
}

[Serializable]
public class GrabSettings
{
    public string TextureName = "_CustomGrabPass";
    public LayerMask includedLayerMask;
}
public class GrabRenderPass : ScriptableRenderPass
{
    private GrabSettings defaultSettings;
    private TextureDesc grabTextureDescriptor;
    private List<ShaderTagId> m_ShaderTagIdList = new List<ShaderTagId>();
    public GrabRenderPass(GrabSettings defaultSettings)
    {
        this.defaultSettings = defaultSettings;
    }

    public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
    {
        UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();
        UniversalRenderingData renderingData = frameData.Get<UniversalRenderingData>();
        UniversalCameraData cameraData = frameData.Get<UniversalCameraData>();
        UniversalLightData lightData = frameData.Get<UniversalLightData>();

        if (resourceData.isActiveTargetBackBuffer)
            return;

        grabTextureDescriptor = resourceData.activeColorTexture.GetDescriptor(renderGraph);
        grabTextureDescriptor.name = defaultSettings.TextureName;
        grabTextureDescriptor.clearBuffer = true;
        int globalTextureID = Shader.PropertyToID(defaultSettings.TextureName);

        TextureHandle customGrabTexture = renderGraph.CreateTexture(grabTextureDescriptor);
        var skyboxRendererList = renderGraph.CreateSkyboxRendererList(cameraData.camera);


        // // draw the skybox because it gets left out of rendererlist
        using (var skyboxPass = renderGraph.AddRasterRenderPass<PassData>("SkyboxPass", out var passData))
        {
            skyboxPass.UseRendererList(skyboxRendererList);
            skyboxPass.SetRenderAttachment(customGrabTexture, 0); // write skybox into the grab texture
            skyboxPass.SetRenderAttachmentDepth(resourceData.activeDepthTexture, AccessFlags.Read);
            skyboxPass.SetRenderFunc((PassData data, RasterGraphContext ctx) =>
            {
                ctx.cmd.DrawRendererList(skyboxRendererList);
            });
        }

        // draw all other meshes
        using (var builder = renderGraph.AddRasterRenderPass<PassData>("TransparentGrabPass", out var passData))
        {
            // layers and queues to render
            RenderQueueRange queueRange = RenderQueueRange.all;
            FilteringSettings filtering = new FilteringSettings(queueRange, defaultSettings.includedLayerMask);

            ShaderTagId[] shaderTags = new ShaderTagId[]
            {
               new ShaderTagId("UniversalForwardOnly"),
                new ShaderTagId("UniversalForward"),
                new ShaderTagId("SRPDefaultUnlit"), // Legacy shaders (do not have a gbuffer pass) are considered forward-only for backward compatibility
                new ShaderTagId("LightweightForward"), // Legacy shaders (do not have a gbuffer pass) are considered forward-only for backward compatibility
            };

            m_ShaderTagIdList.Clear();

            foreach (ShaderTagId sid in shaderTags)
                m_ShaderTagIdList.Add(sid);

            DrawingSettings drawSettings = RenderingUtils.CreateDrawingSettings(
                m_ShaderTagIdList, renderingData, cameraData, lightData, SortingCriteria.CommonTransparent);

            var rendererListParams = new RendererListParams(renderingData.cullResults, drawSettings, filtering);
            passData.rendererListHandle = renderGraph.CreateRendererList(rendererListParams);
            builder.UseRendererList(passData.rendererListHandle);
            builder.SetRenderAttachment(customGrabTexture, 0);
            builder.SetRenderAttachmentDepth(resourceData.activeDepthTexture, AccessFlags.Read);
            builder.SetRenderFunc((PassData data, RasterGraphContext context) =>
            {
                context.cmd.DrawRendererList(data.rendererListHandle);
            });
            builder.SetGlobalTextureAfterPass(customGrabTexture, globalTextureID);

        }
    }

    private class PassData
    {
        public RendererListHandle rendererListHandle;
    }
}