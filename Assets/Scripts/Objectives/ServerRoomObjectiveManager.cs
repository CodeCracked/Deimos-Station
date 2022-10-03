using UnityEngine;

public class ServerRoomObjectiveManager : RoomObjectiveManager
{
    [Range(0.0f, 1.0f)] public float LinkChance = 0.5f;

    public void Start()
    {
        // Create Links
        foreach (Objective objective in Objectives)
        {
            foreach (Objective link in Objectives)
            {
                if (objective is InteractableObjective interactable && link != objective && Random.value < LinkChance)
                {
                    interactable.Interactable.OnInteract.AddListener(() => link.Toggle());
                }
            }
            objective.SetCompleted(true, true);
        }

        // Randomly toggle objectives
        foreach (Objective objective in Objectives) if (objective is InteractableObjective interactable && Random.value < 0.5f) interactable.Interactable.OnInteract.Invoke();

        // Disable Objectives on Completion
        FinalObjective.OnCompleted.AddListener(() => { foreach (Objective objective in Objectives) objective.enabled = false; });
    }
}
