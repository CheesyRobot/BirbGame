using UnityEngine;

public class GrabControl : MonoBehaviour
{
    private bool hasGrabbed;
    private Grabbable grabbedObject;

    void Start() {
        hasGrabbed = false;
    }

    public bool HasGrabbed() {
        return hasGrabbed;
    }

    public Grabbable GetGrabbedObject() {
        return grabbedObject;
    }

    public void setGrabbed(Grabbable grabbedObject, bool value) {
        if (value)
            this.grabbedObject = grabbedObject;
        else
            this.grabbedObject = null;
        hasGrabbed = value;
    }
}
