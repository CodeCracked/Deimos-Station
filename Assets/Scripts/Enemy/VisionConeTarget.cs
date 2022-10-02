using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class VisionConeTarget : MonoBehaviour
{
    public ArtifactManager Artifact;
    public Transform[] TargetPoints;

    public bool IsInDarkness => _lightSources.Count == 0 || !TimerLightManager.AreLightsOn;
    private readonly List<LightTrigger> _lightSources = new();

    public void OnEnable() { VisionCone.AddTarget(this); }
    public void OnDisable() { VisionCone.RemoveTarget(this); }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        foreach (Transform targetPoint in TargetPoints) if (targetPoint) Gizmos.DrawSphere(targetPoint.transform.position, 0.1f);
    }

    public void OnEnterLight(LightTrigger light)
    {
        _lightSources.Add(light);
    }
    public void OnExitLight(LightTrigger light)
    {
        _lightSources.Remove(light);
    }
}
