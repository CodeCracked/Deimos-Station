using UnityEngine;
using UnityEngine.UI;

public class GlowingObjectivesToggle : MonoBehaviour
{
    public Toggle Toggle;

    public void Start()
    {
        Toggle.onValueChanged.AddListener(OnValueChanged);
        Toggle.isOn = OptionsManager.GlowingObjectives;
    }
    public void OnValueChanged(bool value)
    {
        OptionsManager.GlowingObjectives = value;
    }
}
