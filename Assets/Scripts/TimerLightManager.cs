using System.Collections.Generic;
using UnityEngine;

public class TimerLightManager : MonoBehaviour
{
    private static readonly Dictionary<Light, float> _lights = new();
    public static void RegisterLight(Light light){ _lights.Add(light, light.intensity); }

    public float SolidStateTime = 10.0f;
    public float FlickerTime = 1.0f;
    public float FlickerRate = 2.0f;
    public float FlickerStrength = 0.5f;

    private float _timer;
    private bool _turningOn;

    public void OnEnable()
    {
        _turningOn = false;
    }

    public void Update()
    {
        if (_timer > SolidStateTime)
        {
            float t = (_timer - SolidStateTime) / FlickerTime;
            float flickerCoefficient = 0.5f * (Mathf.Cos(2 * (FlickerRate + 0.5f) * Mathf.PI * t) + 1);
            if (_turningOn)
            {
                flickerCoefficient = 1.0f - flickerCoefficient;
                float flickerAmount = flickerCoefficient * FlickerStrength;
                foreach (KeyValuePair<Light, float> lightEntry in _lights) lightEntry.Key.intensity = lightEntry.Value * flickerAmount;
            }
            else
            {
                float flickerAmount = flickerCoefficient * FlickerStrength;
                foreach (KeyValuePair<Light, float> lightEntry in _lights) lightEntry.Key.intensity = lightEntry.Value - lightEntry.Value * flickerAmount;
            }
        }

        _timer += Time.deltaTime;
        while (_timer >= SolidStateTime + FlickerTime)
        {
            if (_turningOn) foreach (KeyValuePair<Light, float> lightEntry in _lights) lightEntry.Key.intensity = lightEntry.Value;
            else foreach (KeyValuePair<Light, float> lightEntry in _lights) lightEntry.Key.intensity = 0.0f;

            _timer -= SolidStateTime + FlickerTime;
            _turningOn = !_turningOn;
        }
    }
}
