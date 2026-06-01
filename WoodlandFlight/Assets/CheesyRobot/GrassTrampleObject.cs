using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GrassTrampleObject : MonoBehaviour
{
    [Tooltip("The renderer settings with the grass trample feature")]
    [SerializeField] private UniversalRendererData rendererSettings = null;

    private bool TryGetFeature(out GrassTramplingFeature feature)
    {
        feature = rendererSettings.rendererFeatures.
            OfType<GrassTramplingFeature>().FirstOrDefault();
        return feature != null;
    }

    private void OnEnable()
    {
        if (TryGetFeature(out GrassTramplingFeature feature))
        {
            feature.AddTrackedTransform(transform);
        }
    }

    private void OnDisable()
    {
        if (TryGetFeature(out GrassTramplingFeature feature))
        {
            feature.RemoveTrackedTransform(transform);
        }
    }
}
