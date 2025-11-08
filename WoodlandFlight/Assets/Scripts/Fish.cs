using UnityEngine;

public class Fish : MonoBehaviour
{
    public bool cought;
    public Grabbable grabbable;
    public FishMovement fishMovement;

    void Start() {
        cought = false;
        grabbable.enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().useGravity = false;
    }

    public void Catch(Transform grabPoint) {
        if (cought)
            return;
        cought = true;
        grabbable.enabled = true;
        fishMovement.enabled = false;
        GetComponent<Rigidbody>().isKinematic = false;
        grabbable.Grab(grabPoint);
    }
}