using Unity.VisualScripting;
using UnityEngine;

public class Hoop : MonoBehaviour
{
    public bool target { get; set; }
    public bool visible { get; set; }
    public bool completed { get; set; }
    // private MeshCollider collider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = false;
        visible = false;
        completed = false;
        // collider = this.gameObject.GetComponent<MeshCollider>();
    }

    void OnDisable() {
        target = false;
        visible = false;
        completed = false;
    }

    public void SetTarget(bool value) {
        target = value;
    }
    
    public void SetVisible(bool value) {
        visible = value;
    }

    private void OnTriggerEnter(Collider other) {
        // detect if other collider is the player
        if (other.GetComponent<Movement>() != null)
            completed = true;
    }
}
