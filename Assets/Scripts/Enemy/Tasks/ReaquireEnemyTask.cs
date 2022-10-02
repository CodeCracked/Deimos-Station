using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ReaquireEnemyTask : AbstractEnemyTask
{
    private readonly VisionConeTarget _target;

    public ReaquireEnemyTask(EnemyController enemy, NavMeshAgent enemyAgent, VisionConeTarget target) : base(enemy, enemyAgent)
    {
        _target = target;
    }

    public override IEnumerator RunTask()
    {
        Enemy.Artifact.State = ArtifactState.Focused;
        yield return LookAroundForTarget();
        yield return SearchForTarget();

    }

    private IEnumerator LookAroundForTarget()
    {
        Vector2 targetHeading = new Vector2(_target.transform.position.x - Enemy.transform.position.x, _target.transform.position.z - Enemy.transform.position.z).normalized;
        float targetAngle = Mathf.Atan2(targetHeading.y, targetHeading.x);
        Ray targetRay = new(Enemy.VisionCone.transform.position, targetHeading);

        // If there is a clear vision line to target, try to find target by looking at them
        if (Physics.Raycast(targetRay, out RaycastHit hit, Enemy.VisionCone.Range))
        {
            VisionConeTarget hitTarget = hit.collider.gameObject.GetComponent<VisionConeTarget>();
            if (!hitTarget) hitTarget = hit.collider.gameObject.GetComponentInChildren<VisionConeTarget>();
            if (hitTarget && hitTarget == _target) yield return LookAtAngle(targetAngle, Enemy.AISettings.ReaquireLookSpeed);
        }

        // If that wasn't successful, do four 180-degree look arcs
        float startingAngle = Enemy.transform.rotation.eulerAngles.y;
        yield return LookAtAngle(startingAngle + 179, Enemy.AISettings.ReaquireLookSpeed);
        yield return new WaitForSeconds(0.5f);
        yield return LookAtAngle(startingAngle, Enemy.AISettings.ReaquireLookSpeed);
        yield return new WaitForSeconds(0.5f);
        yield return LookAtAngle(startingAngle - 179, Enemy.AISettings.ReaquireLookSpeed);
        yield return new WaitForSeconds(0.5f);
        yield return LookAtAngle(startingAngle, Enemy.AISettings.ReaquireLookSpeed);
        yield return new WaitForSeconds(0.5f);

        // Resume Search Mode
        Enemy.SetTask(new SearchEnemyTask(Enemy, EnemyAgent));
    }

    private IEnumerator SearchForTarget()
    {
        yield break;
    }

    public override void OnSensorTriggered(AbstractEnemySensor sensor, object sensorResult)
    {
        if (sensor is VisionConeSensor && sensorResult is VisionConeHit visionHit) Enemy.SetTask(new PursueEnemyTask(Enemy, EnemyAgent, visionHit.Target));
        else if (sensor is ArtifactFocusSensor && sensorResult is VisionConeTarget target) Enemy.SetTask(new PursueEnemyTask(Enemy, EnemyAgent, target));
    }
}