using UnityEngine;
using UnityEngine.UI;

public class BrightnessSlider : MonoBehaviour
{
    public float MinBrightness = 0.25f;
    public float MaxBrightness = 3.0f;
    public Slider Slider;

    public void Start()
    {
        Slider.minValue = MinBrightness;
        Slider.maxValue = MaxBrightness;
        Slider.value = OptionsManager.Brightness;
        Slider.onValueChanged.AddListener(OnValueChanged);
    }
    public void OnValueChanged(float value)
    {
        OptionsManager.Brightness = value;
    }
}
