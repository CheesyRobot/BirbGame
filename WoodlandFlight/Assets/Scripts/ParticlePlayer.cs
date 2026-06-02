using UnityEngine;

public class ParticlePlayer : MonoBehaviour
{
    [SerializeField] private ParticleSystem _rippleParticleSystem;
    private Animator anim;
    [SerializeField] private TrailRenderer _trail1;
    [SerializeField] private TrailRenderer _trail2;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    void OnCollisionEnter(Collision collision)
    {
        
        //Debug.Log("Touched " + collision.transform.root.gameObject.layer);
        if(collision.transform.root.gameObject.layer == 4)
        {
            Vector3 pos = gameObject.transform.position;
            pos.y = 0;
            _rippleParticleSystem.Play();
            
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        //Debug.Log("Untouched " + collision.transform.root.gameObject.layer);
        if (collision.transform.root.gameObject.layer == 4)
        {
            _rippleParticleSystem.Stop();
        }
    }

    private void FixedUpdate()
    {
        if (anim.GetBool("Gliding"))
        {
            if (_trail1.emitting) return;
            _trail1.emitting = true;
            _trail2.emitting = true;
        }
        else
        {
            if (!_trail1.emitting) return;
            _trail1.emitting = false;
            _trail2.emitting = false;
        }

    }

}
