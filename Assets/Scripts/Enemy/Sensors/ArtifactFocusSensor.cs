using UnityEngine;
using UnityEngine.AI;

public class ArtifactFocusSensor : AbstractEnemySensor
{
    public ArtifactFocusSensor(EnemyController enemy, NavMeshAgent enemyAgent, float testInterval) : base(enemy, enemyAgent, testInterval) { }

    protected override bool ExecuteSensor(out object sensorResult)
    {
        VisionConeTarget currentResult = null;
        
        foreach (VisionConeTarget target in VisionCone.Targets)
        {
            // If the target has a focused artifact
            if (target.Artifact && target.Artifact.State == ArtifactState.Focused)
            {
                Vector3 deltaPosition = Enemy.transform.position - target.Artifact.transform.position;

                // If artifact is too far away
                if (deltaPosition.sqrMagnitude > Enemy.AISettings.SenseFocusedArtifactRange * Enemy.AISettings.SenseFocusedArtifactRange) continue;

                // If enemy is outside artifact angle
                Vector3 heading = deltaPosition.normalized;
                float angle = Vector3.Angle(target.Artifact.transform.forward, heading);
                if (angle > target.Artifact.FocusLightAngle * 0.5f) continue;

                // Perform Raycast
                Ray ray = new(target.Artifact.transform.position, heading);
                if (Physics.Raycast(ray, out RaycastHit hit, Enemy.AISettings.SenseFocusedArtifactRange))
                {
                    if (hit.collider.gameObject == Enemy.gameObject)
                    {
                        ApplyResultPriority(ref currentResult, target);
                        break;
                    }
                }
            }
        }

        sensorResult = currentResult;
        return sensorResult != null;
    }

    private void ApplyResultPriority(ref VisionConeTarget existingResult, VisionConeTarget testResult)
    {
        if (!existingResult) existingResult = testResult;
        else
        {
            float currentDistanceSqr = (existingResult.transform.position - Enemy.transform.position).sqrMagnitude;
            float testDistanceSqr = (testResult.transform.position - Enemy.transform.position).sqrMagnitude;
            if (currentDistanceSqr > testDistanceSqr) existingResult = testResult;
        }
    }
}
