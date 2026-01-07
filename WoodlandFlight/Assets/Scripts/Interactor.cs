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
            IInteractable interactable;
            IInteractableModifier interactableMod;
            if (Input.GetKey(KeyCode.LeftShift)) {
                interactableMod = colliders[^1].GetComponent<IInteractableModifier>();
                if (interactableMod != null) {
                    prompt.gameObject.SetActive(true);
                    prompt.SetText("(E) " + interactableMod.InteractionPrompt);
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        interactableMod.Interact(this);
                    }
                }
            }
            else
            {
                interactable = colliders[^1].GetComponent<IInteractable>();
                if (interactable != null) {
                    prompt.gameObject.SetActive(true);
                    prompt.SetText("(E) " + interactable.InteractionPrompt);
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        interactable.Interact(this);
                    }
                }
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

