using TMPro;
using UnityEngine;

public class FishCatcher : MonoBehaviour
{
    public Transform catchPoint;
    [SerializeField] private LayerMask fishLayerMask;
    [SerializeField] private float catchRadius;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip sound;
    public void Catch()
    {
        Collider[] colliders = Physics.OverlapSphere(catchPoint.position, catchRadius, fishLayerMask);
        if (colliders.Length != 0)
        {
            if (colliders[^1].gameObject.tag == "Fish") {
                Fish fish = colliders[^1].GetComponent<Fish>();
                fish?.Catch(catchPoint);
            }
        }
        source.clip = sound;
        source.Play();
    }

    // Show pick-up radius
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(interactionPoint.position, interactionRadius);
    //}
}


