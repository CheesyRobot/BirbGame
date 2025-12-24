using System;
using System.Collections;
using System.Net.Http.Headers;
using Unity.Mathematics;
//using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Splines.Interpolators;

public class Movement : MonoBehaviour
{
    public float verticalForce;
    public float verticalSpeed;
    public float glidingSpeed;
    public float flyingSpeed;
    public float walkingSpeed;
    public float playerHeight;
    public float mass;
    [SerializeField] LayerMask ground;
    [SerializeField] LayerMask water;
    [SerializeField] FishCatcher fishCatcher;
    private Rigidbody rb;
    private Vector3 speed;
    private float linearDamping;
    private float maxSpeed;
    private bool grounded;
    private bool aboveWater;
    private bool swimming;
    private bool gliding;
    private bool flying;
    private bool fishing;
    private bool rebounding;
    private bool enableWalking;
    private float heightOffset;
    private float timer;
    public Transform Camera;
    private Transform tr;
    private Player player;
    Vector2 inputDirection;
    Animator anim;
    GrabControl grabControl;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = GetComponent<Player>();
        tr = GetComponent<Transform>();
        linearDamping = rb.linearDamping;
        mass = rb.mass;
        rb.freezeRotation = true;
        fishing = false;
        rebounding = false;
        aboveWater = false;
        enableWalking = true;
        heightOffset = 0;
        timer = 0;
        anim = GetComponent<Animator>();
        // To set grabbing animation:
        grabControl = GetComponent<GrabControl>();
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Default"), LayerMask.NameToLayer("Water"), true);
    }

    void Update() {
        inputDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        if (aboveWater && Input.GetKeyDown(KeyCode.E)) {
            fishing = true;
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Default"), LayerMask.NameToLayer("Water"), false);
            rb.useGravity = false;
        }
    }

    void FixedUpdate()
    {
        speed = rb.linearVelocity;
        tr = rb.transform;

        Fishing();
        if (!fishing && !rebounding) {
            VerticalMovement();
            Swimming();
        }
        SetMaxSpeed();
        HorizontalMovement();

        rb.linearVelocity = speed;
        SetAnimationBools();
    }
        

    private void VerticalMovement()
    {
        
        grounded = Physics.Raycast(tr.position, Vector3.down, playerHeight * 0.5f + heightOffset, ground);

        if (Input.GetKey(KeyCode.Space)) {
            if (grounded && enableWalking)   //jumping
            {
                rb.AddForce(Vector3.up * verticalForce * 2);
            }
            
            if (player.currentStamina > 0 && enableWalking)  //flying up
            {
                //rb.AddForce(Vector3.up * verticalForce);
                // speed.y += (verticalForce - mass * 5) * 5 * Time.fixedDeltaTime;
                // speed.y = verticalForce - mass * 5;
                // if (speed.y <= 0)
                //     speed.y += 0.5f;
                // else
                    speed.y = Mathf.Lerp(speed.y, verticalSpeed, Time.fixedDeltaTime * 5f);
                gliding = false;
            }
            flying = true;
            player.AddStamina(-player.staminaConsumptionRate * Time.fixedDeltaTime);
        }
        else if (!grounded && Input.GetKey(KeyCode.LeftShift))   //gliding
        {
            speed.y = Mathf.Clamp(speed.y, -1, float.MaxValue);
            gliding = true;
            flying = false;
            player.AddStamina(player.staminaRecoveryRateGliding * Time.fixedDeltaTime);
        }
        else
        {
            flying = false;
            gliding = false;
            player.AddStamina(player.staminaRecoveryRate * Time.fixedDeltaTime);
        }
        
    }

    private void SetMaxSpeed() {
        if (grounded)
            if (enableWalking)
                maxSpeed = walkingSpeed;
            else
                maxSpeed = 0;
        else if (gliding)
                maxSpeed = Mathf.Lerp(maxSpeed, glidingSpeed, Time.fixedDeltaTime * 0.5f);
            else
                maxSpeed = Mathf.Lerp(maxSpeed, flyingSpeed, Time.fixedDeltaTime * 1.5f);
    }

    private void HorizontalMovement() {
        // Vector3 moveDirection = Vector3.Normalize(Camera.forward * inputDirection.y + Camera.right * inputDirection.x);
        Vector3 moveDirection = Vector3.Normalize(Vector3.Normalize(new Vector3(Camera.forward.x, 0, Camera.forward.z)) * inputDirection.y + Camera.right * inputDirection.x);

        // Character rotation
        if (moveDirection.magnitude != 0)
            rb.transform.forward = new Vector3(moveDirection.x, 0, moveDirection.z);
        // Rotate based on camera
        // else
        //     rb.transform.forward = new Vector3(Camera.forward.x, 0, Camera.forward.z);

        // speed.x = Mathf.Lerp(speed.x, moveDirection.x * maxSpeed, Time.fixedTime * 0.1f);
        // speed.z = Mathf.Lerp(speed.z, moveDirection.z * maxSpeed, Time.fixedTime * 0.1f);
        if (moveDirection.magnitude != 0 || grounded) {
            speed.x = moveDirection.x * maxSpeed;
            speed.z = moveDirection.z * maxSpeed;
        }
        else if (!gliding) {
            speed.x = Mathf.Lerp(speed.x, 0f, Time.fixedDeltaTime * 1.5f);
            speed.z = Mathf.Lerp(speed.z, 0f, Time.fixedDeltaTime * 1.5f);
        }
        rb.linearVelocity = speed;
    }

    private void Swimming() {
        bool hasHit = Physics.Raycast(tr.position + Vector3.down * playerHeight * 2f, Vector3.up, 10f, water);
        if (hasHit) {
            swimming = true;
            rb.linearDamping = linearDamping + 2;
            // if (speed.y <= 0)
            //     speed.y += 0.5f;
            // else
            //     speed.y += 0.2f;
            // rb.AddForce(Vector3.up * verticalForce * 1f);
        }
        else {
            swimming = false;
            rb.linearDamping = linearDamping;
        }
    }

    private void Fishing() {
        Vector3 targetDirection = Vector3.Normalize(Vector3.down + rb.transform.forward);
        Vector3 reboundDirection = Vector3.Normalize(Vector3.up + rb.transform.forward);

        RaycastHit hit;
        Ray rayDirection = new Ray(transform.position, targetDirection);
        bool hasHit = Physics.Raycast(rayDirection, out hit, 10.0f);
        if (hasHit && hit.transform.gameObject.layer == LayerMask.NameToLayer("Water"))
            aboveWater = true;
        else
            aboveWater = false;

        // aboveWater = Physics.Raycast(tr.position, targetDirection, playerHeight * 15.0f, water);
        bool contactWater = Physics.Raycast(tr.position, Vector3.down, playerHeight * 0.5f, water);
        if (fishing) {
            timer += Time.deltaTime;
            // speed = targetDirection * flyingSpeed;
            rb.AddForce(targetDirection * verticalForce * 2);
            if (contactWater || timer > 1.5f) {
                fishCatcher.Catch();
                fishing = false;
                rebounding = true;
                Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Default"), LayerMask.NameToLayer("Water"), true);
                // speed.y = 0f;
                timer = 0;
            }
        }
        else if (rebounding) {
            timer += Time.deltaTime;
            // speed = reboundDirection * flyingSpeed;
            rb.AddForce(reboundDirection * verticalForce);
            if (timer > 0.35f) {
                rebounding = false;
                timer = 0;
                rb.useGravity = true;
            }
        }
    }
    
    public void MovePlayer(Vector3 translation) {
        tr.Translate(translation);
    }

    public void AddMass(float mass) {
        rb.mass = this.mass + mass;
    }

    public void ResetMass()
    {
        rb.mass = this.mass;
    }
    
    public void SetWalkingEnabled(bool value, float offset)
    {
        if (!value && player.weightLimit < rb.mass)
        {
            enableWalking = false;
            heightOffset = offset;
        }
        else
        {
            enableWalking = true;
            heightOffset = 0;
        }
    }

    /// <summary>
    /// Setting bools in the animator for animation purposes
    /// </summary>
    public void SetAnimationBools()
    {
        anim.SetBool("Gliding", gliding);
        anim.SetBool("Flying", flying);
        anim.SetBool("Diving", fishing);
        anim.SetBool("Grounded", grounded);
        anim.SetBool("Grabbing", grabControl.HasGrabbed());
        if (speed.z != 0 && speed.x != 0 && grounded && enableWalking)
        {
            anim.SetBool("Walking", true);
        } else anim.SetBool("Walking", false);
    }
}
