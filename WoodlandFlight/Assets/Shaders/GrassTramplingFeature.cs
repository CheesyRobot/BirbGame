using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.RenderGraphModule;
using System.Collections.Generic;
using Unity.Mathematics;
using System.Linq;

public class GrassTramplingFeature : ScriptableRendererFeature
{
    class Pass : ScriptableRenderPass
    {
        private Vector4[] tramplePositions;
        private int numTramplePositions;
        
        public Pass(Vector4[] tramplePositions)
        {
            this.tramplePositions = tramplePositions;
        }

        // This class stores the data needed by the RenderGraph pass.
        // It is passed as a parameter to the delegate function that executes the RenderGraph pass.
        private class PassData
        {
            public Vector4[] tramplePositions;
            public int numTramplePositions;
        }
        public int NumTramplePositions { get => this.numTramplePositions; set => this.numTramplePositions = value; }
        
        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            using (var builder = renderGraph.AddUnsafePass<PassData>("GrassTramplingFeature", out var passData)) {
                passData.tramplePositions = tramplePositions;
                passData.numTramplePositions = numTramplePositions;
                builder.AllowPassCulling(false);
                //Debug.Log(passData.tramplePositions[0] + " record render graph 0");
                builder.SetRenderFunc((PassData data, UnsafeGraphContext context) => {
                    context.cmd.SetGlobalVectorArray("_GrassTramplePositions", data.tramplePositions);
                    context.cmd.SetGlobalInt("_NumGrassTramplePositions", data.numTramplePositions);
                });

            }
        }
    }
    [SerializeField] private int maxTrackedTransforms = 10;

    private Pass pass;
    private List<Transform> trackingTransforms;
    private Vector4[] tramplePositions;

    public void AddTrackedTransform(Transform transform)
    {
        trackingTransforms.Add(transform);
    }

    public void RemoveTrackedTransform(Transform transform)
    {
        trackingTransforms.Remove(transform);
    }

    /// <inheritdoc/>
    public override void Create()
    {
        trackingTransforms = new List<Transform>();
        trackingTransforms.AddRange(FindObjectsByType<GrassTrampleObject>(FindObjectsSortMode.None)
            .Select((o) => o.transform));
        tramplePositions = new Vector4[maxTrackedTransforms];
        pass = new Pass(tramplePositions);
        // Configures where the render pass should be injected.
        pass.renderPassEvent = RenderPassEvent.BeforeRendering;
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
#if UNITY_EDITOR
        // If scenes are changed an object might not be able to
        // remove itself from being tracked
        trackingTransforms.RemoveAll((t) => t == null);

#endif
        // Clear out all positions
        for (int i = 0; i < tramplePositions.Length; i++) {
            tramplePositions[i] = Vector4.zero;
        }

        // Calculate number of active tracked transforms
        int count = (int)Mathf.Min(trackingTransforms.Count, tramplePositions.Length);
        for(int i  = 0; i < count; i++)
        {
            Vector3 posn = trackingTransforms[i].position;
            tramplePositions[i] = new Vector4(posn.x, posn.y, posn.z, 1);
        }
        pass.NumTramplePositions = count;
        renderer.EnqueuePass(pass);
    }

}
