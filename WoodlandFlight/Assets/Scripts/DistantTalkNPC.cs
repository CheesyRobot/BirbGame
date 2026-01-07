using UnityEngine;

public class DistantTalkNPC : MonoBehaviour
{
    [SerializeField] private NPC NPC;

    void OnTriggerEnter(Collider col) {
        if (col.GetComponent<Player>() != null)
        {
            NPC.Interact(col.GetComponent<Interactor>());
        }
    }
}
