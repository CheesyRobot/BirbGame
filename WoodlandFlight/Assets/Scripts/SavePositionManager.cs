using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class SavePositionManager : MonoBehaviour
{
    

    // void Start() {
    //     grabbables = Resources.FindObjectsOfTypeAll<Grabbable>();
    // }
    public SavePositionManagerData Save() {
        SavePosition[] items = Resources.FindObjectsOfTypeAll<SavePosition>();
        SavePositionManagerData savePositionManagerData = new();
        savePositionManagerData.items = items.Where(g => Filter(g)).Select(g => new SavePositionData(g.ID, g.transform.position, g.transform.rotation)).ToList();
        return savePositionManagerData;
    }

    public void Load(SavePositionManagerData savePositionManagerData) {
        SavePosition[] items = Resources.FindObjectsOfTypeAll<SavePosition>();
        foreach (SavePosition item in items) {
            foreach (SavePositionData itemData in savePositionManagerData.items) {
                if (item.ID == itemData.id) {
                    item.transform.position = itemData.position;
                    item.transform.rotation = itemData.rotation;
                    Rigidbody rb = item.GetComponent<Rigidbody>();
                    if (rb != null)
                        rb.linearVelocity = new Vector3(0,0,0);
                }
            }
        }
    }

    private bool Filter(SavePosition item) {
        if (item.GetComponent<Fish>()?.enabled == true)
            return false;
        else
            return true;
    }
}

[System.Serializable]
public struct SavePositionManagerData {
    public List<SavePositionData> items;
}

[System.Serializable]
public struct SavePositionData {
    public string id;
    public Vector3 position;
    public Quaternion rotation;

    public SavePositionData(string id, Vector3 position, Quaternion rotation) {
        this.id = id;
        this.position = position;
        this.rotation = rotation;
    }

}
