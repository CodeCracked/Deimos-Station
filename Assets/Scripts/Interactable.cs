using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public GameObject Label;
    public UnityEvent OnHoverStart;
    public UnityEvent OnHoverEnd;
    public UnityEvent OnInteract;

    protected bool Hovering { get; private set; }
    private bool _hoveredThisFrame;

    public void Awake()
    {
        Label.SetActive(false);
        OnHoverStart.AddListener(() => { if (Label) Label.SetActive(true); });
        OnHoverEnd.AddListener(() => { if (Label) Label.SetActive(false); });
    }

    public void LateUpdate()
    {
        if (Hovering && !_hoveredThisFrame)
        {
            OnHoverEnd?.Invoke();
            Hovering = false;
        }
        _hoveredThisFrame = false;
    }

    public void MarkHoveredThisFrame()
    {
        if (!Hovering) OnHoverStart?.Invoke();
        Hovering = true;
        _hoveredThisFrame = true;
    }
}
