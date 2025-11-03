using UnityEngine;

public class GrabControl : MonoBehaviour
{
    private bool hasGrabbed;

    void Start() {
        hasGrabbed = false;
    }

    public bool HasGrabbed() {
        return hasGrabbed;
    }

    public void setGrabbed(bool value) {
        hasGrabbed = value;
    }
}
