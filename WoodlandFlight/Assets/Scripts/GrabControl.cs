using System.Linq;
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

    public GrabControlData Save() {
        GrabControlData gbd = new GrabControlData();
        
        if (grabbedObject == null)
            return gbd;

        SavePosition sp = grabbedObject.GetComponent<SavePosition>();
       
        if (sp != null)
            gbd.grabbedObject = sp.ID;
        return gbd;
    }

    public void Load(GrabControlData grabControlData) {
        grabbedObject?.Drop();

        if (grabControlData.grabbedObject != "") {
            SavePosition[] items = Resources.FindObjectsOfTypeAll<SavePosition>();
            Grabbable grabbable = items.FirstOrDefault(i => i.ID == grabControlData.grabbedObject).GetComponent<Grabbable>();
            grabbable.Interact(GetComponent<Interactor>());
        }
    }
}



[System.Serializable]
public struct GrabControlData {
    public string grabbedObject;
}
