using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class InteractableObjective : Objective
{
    public Interactable Interactable;

    public void Awake()
    {
        Interactable.OnInteract.AddListener(Toggle);
    }
}
