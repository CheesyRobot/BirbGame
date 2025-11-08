using TMPro;
using UnityEngine;

public class FishCatcher : MonoBehaviour
{
    public Transform catchPoint;
    [SerializeField] private LayerMask fishLayerMask;
    [SerializeField] private float catchRadius;
    [SerializeField] private float escapeRadius;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip sound;
    [SerializeField] private ParticleSystem particles;
    public void Catch()
    {
        source.clip = sound;
        source.Play();
        particles.Play();

        // Successful catch
        Collider[] colliders = Physics.OverlapSphere(catchPoint.position, catchRadius, fishLayerMask);
        if (colliders.Length != 0)
        {
            if (colliders[^1].gameObject.tag == "Fish") {
                Fish fish = colliders[^1].GetComponent<Fish>();
                fish?.Catch(catchPoint);
                return;
            }
        }

        // Unsuccessful catch
        colliders = Physics.OverlapSphere(catchPoint.position, escapeRadius, fishLayerMask);
        if (colliders.Length != 0)
        {
            if (colliders[^1].gameObject.tag == "Fish") {
                FishMovement fish = colliders[^1].GetComponent<FishMovement>();
                fish?.Despawn();
            }
        }
    }

    // Show pick-up radius
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(interactionPoint.position, interactionRadius);
    //}
}


