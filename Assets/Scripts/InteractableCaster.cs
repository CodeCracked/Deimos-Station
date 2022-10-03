using UnityEngine;

public class InteractableCaster : MonoBehaviour
{
    public Camera Camera;
    public string InteractButton = "Interact";
    public float Range = 3.0f;

    public void Update()
    {
        Ray ray = Camera.ViewportPointToRay(0.5f * Vector3.one);
        if (Physics.Raycast(ray, out RaycastHit hit, Range))
        {
            Interactable interactable = hit.collider.gameObject.GetComponent<Interactable>();
            if (interactable && interactable.enabled)
            {
                interactable.MarkHoveredThisFrame();
                if (Input.GetButtonDown(InteractButton))
                {
                    interactable.OnInteract?.Invoke();
                }
            }
        }
    }
}
