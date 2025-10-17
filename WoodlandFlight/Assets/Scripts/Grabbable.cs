using UnityEditor.Rendering;
using UnityEngine;

public class Grabbable : MonoBehaviour
{
    private Rigidbody rb;
    private Transform grabPoint;
    public Vector3 offset;
    private Collider cl;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cl = GetComponent<Collider>();
        offset = Mathf.Min(cl.bounds.size.x, cl.bounds.size.y, cl.bounds.size.z) * 0.5f * Vector3.down;
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

    void FixedUpdate()
    {
        if (grabPoint != null)
        {
            rb.MovePosition(grabPoint.position + offset);
        }
    }
}
