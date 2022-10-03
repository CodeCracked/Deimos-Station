using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public GameObject Label;
    public UnityEvent OnHoverStart;
    public UnityEvent OnHoverEnd;
    public UnityEvent OnInteract;

    private bool _hovering;
    private bool _hoveredThisFrame;

    public void Awake()
    {
        OnHoverStart.AddListener(() => { if (Label) Label.SetActive(true); });
        OnHoverEnd.AddListener(() => { if (Label) Label.SetActive(true); });
    }

    public void LateUpdate()
    {
        if (_hovering && !_hoveredThisFrame)
        {
            OnHoverEnd?.Invoke();
            _hovering = false;
        }
        _hoveredThisFrame = false;
    }

    public void MarkHoveredThisFrame()
    {
        if (!_hovering) OnHoverStart?.Invoke();
        _hovering = true;
        _hoveredThisFrame = true;
    }
}
