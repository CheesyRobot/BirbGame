using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class Grabbable : MonoBehaviour, IInteractable
{
    private Rigidbody rb;
    private Transform grabPoint;
    public Vector3 offset;
    private Collider cl;
    [SerializeField] private string prompt;
    public string InteractionPrompt => prompt;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cl = GetComponent<Collider>();
        offset = Mathf.Min(cl.bounds.size.x, cl.bounds.size.y, cl.bounds.size.z) * 0.5f * Vector3.down;
        prompt = "(E) Pick Up";
    }

    public void Grab(Transform grabPoint)
    {
        this.grabPoint = grabPoint;
        rb.useGravity = false;
    }

    public void Drop() {
        this.grabPoint = null;
        rb.useGravity = true;
    }

    public bool Interact(Interactor interactor)
    {
        if (grabPoint == null)
        {
            grabPoint = interactor.interactionPoint;
            interactor.GetComponent<Movement>().MovePlayer(this.transform.position - grabPoint.position - offset);
            rb.useGravity = false;
            prompt = "(E) Drop";
        }
        else
        {
            grabPoint = null;
            rb.useGravity = true;
            prompt = "(E) Pick Up";
        }
        return true;
    }

    void FixedUpdate()
    {
        if (grabPoint != null)
        {
            rb.MovePosition(grabPoint.position + offset);
        }
    }
}
