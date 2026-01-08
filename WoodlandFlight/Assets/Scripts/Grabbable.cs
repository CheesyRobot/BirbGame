using Unity.VisualScripting;
//using UnityEditor.Rendering;
using System.Linq;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

public class Grabbable : MonoBehaviour, IInteractable
{
    private Rigidbody rb;
    private Transform grabPoint;
    private Vector3 offset;
    public Vector3 grabDirection = Vector3.up;
    public float rotateAngle;
    private char coord;
    private float sign;
    private Collider cl;
    [SerializeField] private float mass;
    [SerializeField] private string prompt;
    public string InteractionPrompt => prompt;
    private GrabControl grabControl;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cl = GetComponent<Collider>();
        float minDist;
        // float minDist = Mathf.Min(cl.bounds.size.x, cl.bounds.size.y, cl.bounds.size.z);
        // if (cl.bounds.size.x == minDist)
        //     coord = 'x';
        // else if (cl.bounds.size.y == minDist)
        //     coord = 'y';
        // else
        //     coord = 'z';
        if (grabDirection.x != 0) {
            coord = 'x';
            minDist = cl.bounds.size.x;
            sign = grabDirection.x;
        }
        else if (grabDirection.y != 0) {
            coord = 'y';
            minDist = cl.bounds.size.y;
            sign = grabDirection.y;
        }
        else {
            coord = 'z';
            minDist = cl.bounds.size.z;
            sign = grabDirection.z;
        }
        offset = minDist * 0.5f * Vector3.down;

    }

    public void Grab(Transform grabPoint)
    {
        this.grabPoint = grabPoint;
        rb.useGravity = false;
        // rb.freezeRotation = true;
        DisableRotation(true);
        grabPoint.gameObject.GetComponentInParent<GrabControl>().setGrabbed(this, true);
        //prompt = "(E) Drop";
    }

    public bool IsGrabbed() {
        if (grabPoint == null)
            return false;
        else
            return true;
    }

    public void Drop() {
        this.grabPoint = null;
        rb.useGravity = true;
        // rb.freezeRotation = false;
        grabControl?.setGrabbed(this, false);
        grabPoint?.gameObject.GetComponentInParent<GrabControl>()?.setGrabbed(this, true);
        DisableRotation(false);
    }

    public bool Interact(Interactor interactor)
    {
        Movement player = interactor.GetComponent<Movement>();
        grabControl = interactor.GetComponent<GrabControl>();

        if (grabPoint == null && !grabControl.HasGrabbed())
        {
            grabPoint = interactor.interactionPoint;
            player.MovePlayer(this.transform.position - grabPoint.position - offset);
            player.AddMass(mass);
            player.SetWalkingEnabled(false, -offset.y * 2f);
            grabControl.setGrabbed(this, true);
            rb.useGravity = false;
            DisableRotation(true);
            // rb.freezeRotation = true;
            //prompt = "(E) Drop";
        }
        else if (grabPoint != null && grabControl.HasGrabbed())
        {
            grabPoint = null;
            rb.useGravity = true;
            // rb.freezeRotation = false;
            DisableRotation(false);
            player.ResetMass();
            player.SetWalkingEnabled(true, 0);
            grabControl.setGrabbed(this, false);
            //prompt = "(E) Pick Up";
        }
        return true;
    }

    void FixedUpdate()
    {
        if (grabPoint != null)
        {
            rb.MovePosition(grabPoint.position + offset);
            if (coord == 'y')
                // rb.transform.forward = grabPoint.transform.forward;
                rb.transform.up = Vector3.up * sign;
            else if (coord == 'x')
                rb.transform.up = Vector3.right * sign;
            // rb.transform.up = grabPoint.transform.forward;
            else
                rb.transform.up = Vector3.forward * sign;
            // rb.transform.right = grabPoint.transform.forward;
            rb.transform.RotateAround(transform.position, Vector3.up, grabPoint.transform.eulerAngles.y + rotateAngle);
        }
    }

    private void DisableRotation(bool value)
    {
        if (value)
        {
            if (coord == 'y')
                rb.constraints = rb.constraints | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
            else if (coord == 'x')
                rb.constraints = rb.constraints | RigidbodyConstraints.FreezeRotationZ;
            else
                rb.constraints = rb.constraints | RigidbodyConstraints.FreezeRotationX;
        }
        else
            rb.constraints = rb.constraints & ~(RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX);
    }
}
