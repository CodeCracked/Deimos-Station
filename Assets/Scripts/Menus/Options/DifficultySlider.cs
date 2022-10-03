using UnityEngine;
using UnityEngine.UI;

public class DifficultySlider : MonoBehaviour
{
    public int MinDifficulty = 0;
    public int MaxDifficulty = 6;
    public Slider Slider;

    public void Start()
    {
        Slider.minValue = MinDifficulty;
        Slider.maxValue = MaxDifficulty;
        Slider.wholeNumbers = true;
        Slider.value = OptionsManager.Difficulty;
        Slider.onValueChanged.AddListener(OnValueChanged);
    }
    public void OnValueChanged(float value)
    {
        OptionsManager.Difficulty = (int)value;
    }
}
