using UnityEngine;
using UnityEngine.UI;

public class MouseSensitivitySlider : MonoBehaviour
{
    public enum MouseAxis { X, Y }

    public float MinSensitivity = 0.5f;
    public float MaxSensitivity = 5.0f;
    public MouseAxis Axis = MouseAxis.X;
    public Slider Slider;

    public void Start()
    {
        Slider.minValue = MinSensitivity;
        Slider.maxValue = MaxSensitivity;
        Slider.value = Axis == MouseAxis.X ? OptionsManager.MouseXSensitivity : OptionsManager.MouseYSensitivity;
        Slider.onValueChanged.AddListener(OnValueChanged);
    }
    public void OnValueChanged(float value)
    {
        if (Axis == MouseAxis.X) OptionsManager.MouseXSensitivity = value;
        else OptionsManager.MouseYSensitivity = value;
    }
}
