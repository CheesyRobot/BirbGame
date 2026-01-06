using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Bonk : MonoBehaviour
{
    [SerializeField] private float minimumRequiredSpeed;
    [SerializeField] private Animator animator;
    public event EventHandler OnBonked;

    void OnTriggerEnter(Collider col) {
        // will need to exclude player from this, but for now keeping it in for easier bonking
        if (col.GetComponent<Rigidbody>() != null)
            if (Vector3.Magnitude(col.GetComponent<Rigidbody>().linearVelocity) >= minimumRequiredSpeed) {
                animator.SetTrigger("Bonk");
                animator.SetBool("isWalking", false);
                OnBonked?.Invoke(this, EventArgs.Empty);
            }
    }
}
