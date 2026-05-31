using UnityEngine;

public class ParticlePlayer : MonoBehaviour
{
    [SerializeField] private ParticleSystem _rippleParticleSystem;

    void OnCollisionEnter(Collision collision)
    {
        
        Debug.Log("Touched " + collision.transform.root.gameObject.layer);
        if(collision.transform.root.gameObject.layer == 4)
        {
            Vector3 pos = gameObject.transform.position;
            pos.y = 0;
            _rippleParticleSystem.Play();
            
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("Untouched " + collision.transform.root.gameObject.layer);
        if (collision.transform.root.gameObject.layer == 4)
        {
            _rippleParticleSystem.Stop();
        }
    }

}
