using Unity.VisualScripting;
using UnityEngine;

public class Hoop : MonoBehaviour
{
    public bool target { get; set; }
    public bool completed { get; set; }
    void Start()
    {
        target = false;
        completed = false;
    }

    void OnDisable() {
        target = false;
        completed = false;
    }

    public void SetTarget(bool value) {
        target = value;
    }

    private void OnTriggerEnter(Collider other) {
        // detect if other collider is the player
        if (other.GetComponent<Movement>() != null)
            completed = true;
    }
}
