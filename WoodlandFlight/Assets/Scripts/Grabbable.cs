using Unity.VisualScripting;
//using UnityEditor.Rendering;
using System.Linq;
using UnityEngine;

public class Grabbable : MonoBehaviourID, IInteractable
{
    private Rigidbody rb;
    private Transform grabPoint;
    public Vector3 offset;
    public float rotateAngle;
    private char coord;
    private Collider cl;
    [SerializeField] private float mass;
    [SerializeField] private string prompt;
    public string InteractionPrompt => prompt;
    private GrabControl grabControl;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cl = GetComponent<Collider>();
        float minDist = Mathf.Min(cl.bounds.size.x, cl.bounds.size.y, cl.bounds.size.z);
        offset = minDist * 0.5f * Vector3.down;
        if (cl.bounds.size.x == minDist)
            coord = 'x';
        else if (cl.bounds.size.y == minDist)
            coord = 'y';
        else
            coord = 'z';

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
        if (grabControl != null)
            grabControl.setGrabbed(this, false);
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
                rb.transform.forward = Vector3.up;
            else if (coord == 'x')
                rb.transform.right = Vector3.up;
            // rb.transform.up = grabPoint.transform.forward;
            else
                rb.transform.forward = Vector3.up;
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
