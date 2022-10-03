using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class OptionsManager : MonoBehaviour
{
    #region Singleton Initialization
    public static OptionsManager Instance;
    public void Awake()
    {
        if (Instance) Destroy(gameObject);
        else DontDestroyOnLoad(gameObject);

        Instance = this;
        Brightness = PlayerPrefs.GetFloat("Brightness", Brightness);
        MouseXSensitivity = PlayerPrefs.GetFloat("MouseXSensitivity", MouseXSensitivity);
        MouseYSensitivity = PlayerPrefs.GetFloat("MouseYSensitivity", MouseYSensitivity);
    }
    public void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat("Brightness", Brightness);
        PlayerPrefs.SetFloat("MouseXSensitivity", MouseXSensitivity);
        PlayerPrefs.SetFloat("MouseYSensitivity", MouseYSensitivity);
    }
    #endregion

    public PostProcessProfile[] PostProcessingProfiles;

    public static float Brightness
    {
        get => _brightness;
        set
        {
            _brightness = value;
            if (Instance) foreach (PostProcessProfile profile in Instance.PostProcessingProfiles) if (profile.TryGetSettings(out AutoExposure exposure)) exposure.keyValue.value = _brightness;
        }
    }
    public static float MouseXSensitivity = 1.5f;
    public static float MouseYSensitivity = 1.5f;

    private static float _brightness = 1.0f;
}
