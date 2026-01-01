using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform[] positions;
    [SerializeField] private float waitInPositionMinSeconds;
    [SerializeField] private float waitInPositionMaxSeconds;
    [SerializeField] private float movementSpeed;
    private float relocateTimer;
    private Vector3 targetPosition;
    private Transform targetTransform;
    private Rigidbody rb;
    private bool enableMovement;
    private Animator animator;

    // private Vector3 speed;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        enableMovement = false;
        relocateTimer = waitInPositionMinSeconds;
    }

    void Update()
    {
        // speed = rb.linearVelocity;
        if (Move()) {
            enableMovement = false;
            animator.SetBool("isWalking", false);
            // transform.position = targetTransform.position;
            // transform.rotation = targetTransform.rotation;
            StartCoroutine(Turn(Vector3.SignedAngle(transform.forward, targetTransform.forward, Vector3.up), 2));

        }

        if (!enableMovement && relocateTimer > 0) {
            relocateTimer -= Time.deltaTime;
        }
        else if (!enableMovement) {
            SetTargetTransform(PickRandomTransform());
            relocateTimer = UnityEngine.Random.Range(waitInPositionMinSeconds, waitInPositionMaxSeconds);
            enableMovement = true;
        }
        // rb.linearVelocity = speed;
    }

    public void SetTargetTransform(Transform position) {
        targetPosition = position.position;
        targetTransform = position;
        Debug.Log(targetTransform);
    }

    // returns true if target position is reached
    private bool Move() {
        if (!enableMovement)
            return false;
        
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0;
        if (Vector3.Magnitude(direction) <= 0.1)
            return true;
        
        direction = Vector3.Normalize(direction);
        // direction.x = Mathf.Lerp(transform.forward.x, direction.x, Time.deltaTime * 0.5f);
        // direction.z = Mathf.Lerp(transform.forward.z, direction.z, Time.deltaTime * 0.5f);
        // transform.position += direction * movementSpeed * Time.deltaTime;
        rb.linearVelocity = new Vector3(direction.x * movementSpeed, 0, direction.z * movementSpeed);
        transform.forward = direction;
        animator.SetBool("isWalking", true);
        return false;
    }

    private Transform PickRandomTransform() {
        return positions[UnityEngine.Random.Range(0, positions.Length)];
    }

    IEnumerator Turn(float angle, float speed)
    {
        float angleRotated = 0;
        while (angleRotated < Math.Abs(angle))
        {
            transform.RotateAround(transform.position, Vector3.up, Math.Sign(angle) * speed);
            angleRotated += speed;
            yield return new WaitForSeconds(.02f);
        }
    }
}
