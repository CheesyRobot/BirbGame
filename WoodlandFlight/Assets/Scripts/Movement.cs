using System;
using Unity.Mathematics;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Splines.Interpolators;

public class Movement : MonoBehaviour
{
    public float verticalForce;
    public float glidingSpeed;
    public float flyingSpeed;
    public float walkingSpeed;
    public float playerHeight;
    [SerializeField] LayerMask ground;
    private Rigidbody rb;
    private Vector3 speed;
    private float maxSpeed;
    private float mass;
    private bool grounded;
    private bool gliding;
    public Transform Camera;
    private Transform tr;
    private Player player;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GetComponent<Player>();
        mass = rb.mass;
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        Move();
        rb.mass = mass;
    }
        

    private void Move()
    {
        tr = rb.transform;
        speed = rb.linearVelocity;

        // Flying up
        if (Input.GetKey(KeyCode.Space) && player.currentStamina > 0)
        {
            rb.AddForce(Vector3.up * verticalForce);
            if (speed.y <= 0)
                speed.y += 0.1f;
            gliding = false;
            player.AddStamina(-player.staminaConsumptionRate * Time.fixedDeltaTime);
        }
        // Gliding
        else if (Input.GetKey(KeyCode.LeftShift)) {
            speed.y = Mathf.Clamp(speed.y, -1, float.MaxValue);
            gliding = true;
            player.AddStamina(player.staminaRecoveryRateGliding * Time.fixedDeltaTime);
        }
        else {
            gliding = false;
            player.AddStamina(player.staminaRecoveryRate * Time.fixedDeltaTime);
        }

        grounded = Physics.Raycast(tr.position, Vector3.down, playerHeight * 0.5f, ground);
        
        if (grounded)
            maxSpeed = walkingSpeed;
        else if (gliding)
            maxSpeed = Mathf.Lerp(maxSpeed, glidingSpeed, Time.fixedDeltaTime * 1.0f);
        else
            maxSpeed = Mathf.Lerp(maxSpeed, flyingSpeed, Time.fixedDeltaTime * 1.5f);

        Vector2 inputDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        Vector3 moveDirection = Camera.forward * inputDirection.y + Camera.right * inputDirection.x;

        // Character rotation
        if (moveDirection.magnitude != 0)
            rb.transform.forward = new Vector3(moveDirection.x, 0, moveDirection.z);
        // Rotate based on camera
        // else
        //     rb.transform.forward = new Vector3(Camera.forward.x, 0, Camera.forward.z);

        // speed.x = Mathf.Lerp(speed.x, moveDirection.x * maxSpeed, Time.fixedTime * 0.1f);
        // speed.z = Mathf.Lerp(speed.z, moveDirection.z * maxSpeed, Time.fixedTime * 0.1f);
        speed.x = moveDirection.x * maxSpeed;
        speed.z = moveDirection.z * maxSpeed;
        rb.linearVelocity = new Vector3(speed.x, speed.y, speed.z);
    }
    
    public void MovePlayer(Vector3 translation) {
        tr.Translate(translation);
    }
}
