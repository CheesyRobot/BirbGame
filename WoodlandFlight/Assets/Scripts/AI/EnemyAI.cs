using System;
using System.Collections;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private PlayerDetector playerDetector;
    [SerializeField] private AimConstraint aimConstraint;
    [SerializeField] private Transform[] positions;
    [SerializeField] private float waitInPositionMinSeconds;
    [SerializeField] private float waitInPositionMaxSeconds;
    [SerializeField] private float movementSpeed;
    [SerializeField] private bool isStationary;
    [SerializeField] private AudioSource SFXSource;
    [SerializeField] private AudioClip gunShotSound;
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
        if (playerDetector != null)
            playerDetector.OnPlayerDetected += Receive_OnPlayerDetected;
        if (aimConstraint != null)
            aimConstraint.constraintActive = false;
    }

    void Update()
    {
        if (!isStationary)
            MovementUpdate();
        else
            animator.SetBool("isSitting", true);
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

    private void MovementUpdate() {
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

    IEnumerator Shoot(Vector3 direction, Player player)
    {
        enableMovement = false;
        rb.linearVelocity = Vector3.zero;
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(Turn(Vector3.SignedAngle(transform.forward, direction, Vector3.up), 10));
        animator.SetTrigger("Shoot");
        aimConstraint.constraintActive = true;
        yield return new WaitForSeconds(0.8f);
        SFXSource.clip = gunShotSound;
        SFXSource.Play();
        yield return new WaitForSeconds(0.05f);
        if (playerDetector.IsInSight())
            player.Death();
        aimConstraint.constraintActive = false;
        enableMovement = true;
    }
    private void Receive_OnPlayerDetected(object sender, PlayerDetector.OnPlayerDetectedEventArgs e) {
        // Debug.Log("Detected");
        
        StartCoroutine(Shoot(Vector3.Normalize(e.player.position - transform.position), e.player.GetComponent<Player>()));
    }
}
