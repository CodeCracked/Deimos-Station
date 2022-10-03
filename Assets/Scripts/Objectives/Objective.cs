using UnityEngine;
using UnityEngine.Events;

public class Objective : MonoBehaviour
{
    public bool Completed = false;
    public bool CanRevert = false;
    [ColorUsage(true, true)] public Color StartingColor = 2 * Color.red;
    [ColorUsage(true, true)] public Color MetColor = 2 * Color.green;
    public MeshRenderer Renderer;
    public UnityEvent OnCompleted;
    public UnityEvent OnReverted;

    public void Start()
    {
        Renderer.material.SetColor("_EmissionColor", Completed ? MetColor : StartingColor);
    }

    public void SetCompleted(bool completed = true, bool force = false)
    {
        if (!enabled && !force) return;
        if (Completed && !completed && !CanRevert && !force) return;
        if (Completed != completed)
        {
            if (completed) OnCompleted.Invoke();
            else OnReverted.Invoke();
            Completed = completed;
        }
        Renderer.material.SetColor("_EmissionColor", Completed ? MetColor : StartingColor);
    }
    public void Toggle()
    {
        SetCompleted(!Completed);
    }
}
