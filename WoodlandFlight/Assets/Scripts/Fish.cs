using UnityEngine;

public class Fish : MonoBehaviour
{
    public bool cought;
    public Grabbable component;

    void Start() {
        cought = false;
        component.enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().useGravity = false;
    }

    public void Catch(Transform grabPoint) {
        if (cought)
            return;
        cought = true;
        component.enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
        component.Grab(grabPoint);
    }
}