using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableManager : MonoBehaviour
{
    

    // void Start() {
    //     grabbables = Resources.FindObjectsOfTypeAll<Grabbable>();
    // }
    public GrabbableManagerData Save() {
        Grabbable[] grabbables = Resources.FindObjectsOfTypeAll<Grabbable>();
        GrabbableManagerData grabbableManagerData = new();
        grabbableManagerData.grabbables = grabbables.Where(g => Filter(g)).Select(g => new GrabbableData(g.ID, g.transform.position, g.transform.rotation)).ToList();
        return grabbableManagerData;
    }

    public void Load(GrabbableManagerData grabbableManagerData) {
        Grabbable[] grabbables = Resources.FindObjectsOfTypeAll<Grabbable>();
        foreach (Grabbable grabbable in grabbables) {
            foreach (GrabbableData grabbableData in grabbableManagerData.grabbables) {
                if (grabbable.ID == grabbableData.id) {
                    grabbable.transform.position = grabbableData.position;
                    grabbable.transform.rotation = grabbableData.rotation;
                }
            }
        }
    }

    private bool Filter(Grabbable grabbable) {
        if (grabbable.GetComponent<Fish>()?.enabled == true)
            return false;
        else
            return true;
    }
}

[System.Serializable]
public struct GrabbableManagerData {
    public List<GrabbableData> grabbables;
}

[System.Serializable]
public struct GrabbableData {
    public string id;
    public Vector3 position;
    public Quaternion rotation;

    public GrabbableData(string id, Vector3 position, Quaternion rotation) {
        this.id = id;
        this.position = position;
        this.rotation = rotation;
    }

}
