using UnityEngine;

public class Objective : Interactable
{
    public static int ObjectivesRemaining { get; private set; }

    public bool Completed = false;
    public bool CanRevert = false;
    [ColorUsage(true, true)] public Color StartingColor = 2 * Color.red;
    [ColorUsage(true, true)] public Color MetColor = 2 * Color.green;
    public MeshRenderer Renderer;

    public void Start()
    {
        ObjectivesRemaining++;
        OnInteract.AddListener(InteractListener);

        if (Completed) ObjectivesRemaining--;
        Renderer.material.SetColor("_EmissionColor", Completed ? MetColor : StartingColor);
    }

    public void SetCompleted(bool completed = true)
    {
        if (Completed != completed)
        {
            if (completed) ObjectivesRemaining--;
            else ObjectivesRemaining++;
            Completed = completed;
        }
        Renderer.material.SetColor("_EmissionColor", Completed ? MetColor : StartingColor);
    }
    public void Toggle()
    {
        SetCompleted(!Completed);
    }

    public void InteractListener()
    {
        if (!Completed || CanRevert) Toggle();
    }
}
