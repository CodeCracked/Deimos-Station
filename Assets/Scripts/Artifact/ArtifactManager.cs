using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ArtifactManager : MonoBehaviour
{
    public Light AreaLight;
    public Light FocusLight;
    public float FadeTime = 0.25f;
    public float FocusTransitionAngle = 180.0f;
    public UnityEvent<ArtifactState> OnStateChange = new();

    public ArtifactState State
    {
        get => _state;
        set
        {
            if (_state != value)
            {
                StopAllCoroutines();
                OnStateChange.Invoke(value);
                _state = value;

                switch (_state)
                {
                    case ArtifactState.Concealed:
                        StartCoroutine(DisableLight(AreaLight, _areaLightIntensity));
                        StartCoroutine(DisableLight(FocusLight, _focusLightIntensity));
                        break;
                    case ArtifactState.Focused:
                        StartCoroutine(DisableLight(AreaLight, _areaLightIntensity));
                        StartCoroutine(EnableLight(FocusLight, _focusLightIntensity));
                        break;
                    case ArtifactState.Area:
                        StartCoroutine(EnableLight(AreaLight, _areaLightIntensity));
                        StartCoroutine(DisableLight(FocusLight, _focusLightIntensity));
                        break;
                }
            }
        }
    }

    private ArtifactState _state;
    private float _areaLightIntensity;
    private float _focusLightIntensity;
    private float _focusLightAngle;

    public void Awake()
    {
        _areaLightIntensity = AreaLight.intensity;
        _focusLightIntensity = FocusLight.intensity;
        _focusLightAngle = FocusLight.spotAngle;

        FocusLight.intensity = 0;
        FocusLight.gameObject.SetActive(true);
    }

    private IEnumerator DisableLight(Light light, float intensity)
    {
        light.spotAngle = _focusLightAngle;
        while (light.intensity > 0)
        {
            light.intensity -= Time.deltaTime * intensity / FadeTime;
            light.spotAngle += Time.deltaTime * (FocusTransitionAngle - _focusLightAngle) / FadeTime;
            if (light.intensity <= 0)
            {
                light.intensity = 0;
                light.spotAngle = FocusTransitionAngle;
                break;
            }
            else yield return null;
        }
    }
    private IEnumerator EnableLight(Light light, float intensity)
    {
        light.spotAngle = FocusTransitionAngle;
        while (light.intensity < intensity)
        {
            light.intensity += Time.deltaTime * intensity / FadeTime;
            light.spotAngle -= Time.deltaTime * (FocusTransitionAngle - _focusLightAngle) / FadeTime;
            if (light.intensity >= intensity)
            {
                light.intensity = intensity;
                light.spotAngle = _focusLightAngle;
                break;
            }
            else yield return null;
        }
    }
}

public enum ArtifactState
{
    Area,
    Focused,
    Concealed
}