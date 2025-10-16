using System;
using Unity.Mathematics;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering;

public class Movement : MonoBehaviour
{
    public float verticalForce;
    public float horizontalForce;
    public float glideforce;
    private Rigidbody rb;
    private ConstantForce cf;
    private Vector3 speed;
    private float mass;

    public Transform Camera;
    private Vector3 cameraDirection;
    private Transform transform;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cf = GetComponent<ConstantForce>();
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
        //transform = GetComponent<Transform>();
        transform = rb.transform;
        speed = rb.linearVelocity;


        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddForce(new Vector3(0, verticalForce, 0));
            if (speed.y <= 0)
                speed.y += 1;
            //speed.y = Mathf.Clamp(speed.y, 0, float.MaxValue);
        }
        else if (Input.GetKey(KeyCode.LeftShift))
            //rb.mass = mass / 5;
            speed.y = Mathf.Clamp(speed.y, -1, float.MaxValue);
        //GetComponent<Transform>().RotateAround(transform.position, Vector3.up, Vector2.Dot(new Vector2(Camera.main.transform.forward.x, Camera.main.transform.forward.z), new Vector2(transform.forward.x, transform.forward.z)));
        Vector2 inputDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        //rb.transform.RotateAround(transform.position, Vector3.up, 1);

        Vector3 moveDirection = Camera.forward * inputDirection.y + Camera.right * inputDirection.x;

        if (moveDirection.magnitude != 0)
            rb.transform.forward = new Vector3(moveDirection.x, 0, moveDirection.z);
        // else
        //     rb.transform.forward = new Vector3(Camera.forward.x, 0, Camera.forward.z);

        speed.x = moveDirection.x * horizontalForce;
        speed.z = moveDirection.z * horizontalForce;
        // speed.x = Input.GetAxis("Horizontal") * horizontalForce;
        // speed.z = Input.GetAxis("Vertical") * horizontalForce;
        rb.linearVelocity = new Vector3(speed.x, speed.y, speed.z);
        //rb.linearDamping = 2;
        //rb.AddRelativeForce(0, 0, inputDirection.y * horizontalForce);
        //rb.AddRelativeForce(inputDirection.x * horizontalForce, 0, 0);
        //rb.AddForce(0, 0, moveDirection.y * horizontalForce);
        //rb.AddForce(moveDirection.x * horizontalForce, 0, 0);
        //rb.AddForce(moveDirection * horizontalForce, ForceMode.Force);
    }
}
