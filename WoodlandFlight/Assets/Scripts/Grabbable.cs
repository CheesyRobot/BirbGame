using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class Grabbable : MonoBehaviour, IInteractable
{
    private Rigidbody rb;
    private Transform grabPoint;
    public Vector3 offset;
    private Collider cl;
    [SerializeField] private float mass;
    [SerializeField] private string prompt;
    public string InteractionPrompt => prompt;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cl = GetComponent<Collider>();
        offset = Mathf.Min(cl.bounds.size.x, cl.bounds.size.y, cl.bounds.size.z) * 0.5f * Vector3.down;
    }

    public void Grab(Transform grabPoint)
    {
        this.grabPoint = grabPoint;
        rb.useGravity = false;
        //prompt = "(E) Drop";
    }

    public void Drop() {
        this.grabPoint = null;
        rb.useGravity = true;
    }

    public bool Interact(Interactor interactor)
    {
        Movement player = interactor.GetComponent<Movement>();
        GrabControl grabControl = interactor.GetComponent<GrabControl>();

        if (grabPoint == null && !grabControl.HasGrabbed())
        {
            grabPoint = interactor.interactionPoint;
            player.MovePlayer(this.transform.position - grabPoint.position - offset);
            player.AddMass(mass);
            grabControl.setGrabbed(true);
            rb.useGravity = false;
            //prompt = "(E) Drop";
        }
        else if (grabPoint != null && grabControl.HasGrabbed())
        {
            grabPoint = null;
            rb.useGravity = true;
            player.ResetMass();
            grabControl.setGrabbed(false);
            //prompt = "(E) Pick Up";
        }
        return true;
    }

    void Update()
    {
        if (grabPoint != null)
        {
            rb.MovePosition(grabPoint.position + offset);
            // rb.transform.forward = grabPoint.transform.forward;
        }
    }
}
