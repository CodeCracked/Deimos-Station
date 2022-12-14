using System.Collections.Generic;
using UnityEngine.AI;

public class VisionConeSensor : AbstractEnemySensor
{
    private readonly VisionCone _visionCone;
    private readonly ArtifactManager _artifact;

    public VisionConeSensor(EnemyController enemy, NavMeshAgent enemyAgent, float testInterval) : base(enemy, enemyAgent, testInterval)
    {
        _visionCone = enemy.GetComponentInChildren<VisionCone>();
        _artifact = enemy.Artifact;
    }

    protected override bool ExecuteSensor(out object sensorResult)
    {
        List<VisionConeHit> results = _visionCone.CalculateResults(false);
        VisionConeHit currentResult = null;

        foreach (VisionConeHit hit in results)
        {
            // If target is in Blindsight radius, it is always sensed
            if (hit.Distance <= Enemy.AISettings.Blindsight)
            {
                ApplyResultPriority(ref currentResult, hit);
                continue;
            }

            // If Enemy Artifact is Focused
            if (_artifact.State == ArtifactState.Focused)
            {
                ApplyResultPriority(ref currentResult, hit);
                continue;
            }

            // If Enemy Artifact isn't Focused
            else
            {
                // If target artifact is in area mode, it is sensed
                if (hit.Artifact && hit.Artifact.State == ArtifactState.Area)
                {
                    ApplyResultPriority(ref currentResult, hit);
                    continue;
                }
                
                // If target is not in darkness, it is sensed
                else if (!hit.InDarkness)
                {
                    ApplyResultPriority(ref currentResult, hit);
                    continue;
                }
            }
        }

        sensorResult = currentResult;
        return sensorResult != null;
    }

    private void ApplyResultPriority(ref VisionConeHit existingResult, VisionConeHit testResult)
    {
        if (existingResult == null || existingResult.Distance > testResult.Distance) existingResult = testResult;
    }
}
