using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerGrabDrop : MonoBehaviour
{
    [SerializeField] private Transform grabPoint;
    [SerializeField] private LayerMask grabbableLayerMask;
    [SerializeField] private float interactionRadius;
    private Collider collider;
    private Grabbable grabbable;

    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(grabPoint.position, interactionRadius, grabbableLayerMask);
        if (colliders.Length != 0)
        {
            collider = colliders[0];
            //prompt.gameObject.SetActive(true);
            //prompt.SetText("(E) Pick Up");
            if (grabbable == null && Input.GetKeyDown(KeyCode.E))
            {
                if (grabbable == null)
                {
                    grabbable = collider.GetComponent<Grabbable>();
                    this.transform.position = grabbable.transform.position - grabbable.offset;
                    grabbable.Grab(grabPoint);
                }
            }
            else if (grabbable != null && Input.GetKeyDown(KeyCode.E))
            {
                grabbable.Drop();
                grabbable = null;
            }
        }
    }
}
