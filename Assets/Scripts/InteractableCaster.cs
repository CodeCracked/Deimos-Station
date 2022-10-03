using UnityEngine;

public class InteractableCaster : MonoBehaviour
{
    public Camera Camera;
    public string InteractButton = "Interact";

    public void Update()
    {
        Ray ray = Camera.ViewportPointToRay(Vector3.zero);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Interactable interactable = hit.collider.gameObject.GetComponent<Interactable>();
            if (interactable && interactable.enabled)
            {
                interactable.MarkHoveredThisFrame();
                if (Input.GetButtonDown(InteractButton)) interactable.OnInteract?.Invoke();
            }
        }
    }
}
