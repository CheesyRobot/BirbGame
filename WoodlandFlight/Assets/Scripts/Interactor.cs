using TMPro;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public Transform interactionPoint;
    // [SerializeField] private Transform interactionPoint;
    [SerializeField] private LayerMask interactableLayerMask;
    [SerializeField] private float interactionRadius;
    [SerializeField] private TextMeshProUGUI prompt;
    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(interactionPoint.position, interactionRadius, interactableLayerMask);
        if (colliders.Length != 0)
        {
            var interactable = colliders[^1].GetComponent<IInteractable>();
            prompt.gameObject.SetActive(true);
            prompt.SetText(interactable.InteractionPrompt);
            if (interactable != null && Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Interaction");
                interactable.Interact(this);
            }
            
        }
        else
        {
            prompt.gameObject.SetActive(false);
        }
    }

    // Show pick-up radius
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(interactionPoint.position, interactionRadius);
    //}
}

