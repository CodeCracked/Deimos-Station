using System.Collections;
using UnityEngine;

public class ArtifactManager : MonoBehaviour
{
    public PlayerController Player;
    public Light AreaLight;
    public Light FocusLight;
    public string FocusButton = "Fire1";
    public string ConcealButton = "Fire2";
    public float FadeTime = 0.25f;
    public float FocusTransitionAngle = 180.0f;

    public ArtifactState State
    {
        get => _state;
        set
        {
            if (_state != value)
            {
                if (value == ArtifactState.Focused) Player.Speed *= 0.5f;
                else if (_state == ArtifactState.Focused) Player.Speed *= 2;
            }
            _state = value;
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

    public void Update()
    {
        if (Input.GetButton(ConcealButton))
        {
            if (State != ArtifactState.Concealed)
            {
                StopAllCoroutines();
                State = ArtifactState.Concealed;
                StartCoroutine(DisableLight(AreaLight, _areaLightIntensity));
                StartCoroutine(DisableLight(FocusLight, _focusLightIntensity));
            }
        }
        else if (Input.GetButton(FocusButton))
        {
            if (State != ArtifactState.Focused)
            {
                StopAllCoroutines();
                State = ArtifactState.Focused;
                StartCoroutine(DisableLight(AreaLight, _areaLightIntensity));
                StartCoroutine(EnableLight(FocusLight, _focusLightIntensity));
            }
        }
        else
        {
            if (State != ArtifactState.Area)
            {
                StopAllCoroutines();
                State = ArtifactState.Area;
                StartCoroutine(EnableLight(AreaLight, _areaLightIntensity));
                StartCoroutine(DisableLight(FocusLight, _focusLightIntensity));
            }
        }
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