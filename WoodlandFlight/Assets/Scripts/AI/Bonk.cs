using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Bonk : MonoBehaviour
{
    [SerializeField] private float minimumRequiredSpeed;
    [SerializeField] private Animator animator;

    void OnTriggerEnter(Collider col) {
        // will need to exclude player from this, but for now keeping it in for easier bonking
        if (Vector3.Magnitude(col.GetComponent<Rigidbody>().linearVelocity) >= minimumRequiredSpeed) {
            animator.SetTrigger("Bonk");
            animator.SetBool("isWalking", false);
        }
    }
}
