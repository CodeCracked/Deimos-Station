using UnityEngine;

public class TimerLight : MonoBehaviour
{
    public void Awake()
    {
        TimerLightManager.RegisterLight(GetComponent<Light>());
    }
}
