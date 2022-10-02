using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PursueEnemyTask : AbstractEnemyTask
{
    private readonly VisionConeTarget _target;

    private Vector3 _lastSpottedPosition;

    public PursueEnemyTask(EnemyController enemy, NavMeshAgent enemyAgent, VisionConeTarget target) : base(enemy, enemyAgent)
    {
        _target = target;
    }

    public override IEnumerator RunTask()
    {
        Enemy.Artifact.State = ArtifactState.Focused;

        NavMeshPath path = new();
        _lastSpottedPosition = _target.transform.position;
        EnemyAgent.speed = Enemy.AISettings.PersueSpeed;
        EnemyAgent.CalculatePath(_lastSpottedPosition, path);
        EnemyAgent.SetPath(path);

        while (true)
        {
            // If target is visible, pursue
            if (Enemy.VisionCone.CanSeeTarget(_target))
            {
                EnemyAgent.CalculatePath(_lastSpottedPosition, path);
                EnemyAgent.SetPath(path);
                _lastSpottedPosition = _target.transform.position;
            }

            // If target is not visible, try to reaquire
            else
            {
                while (!EnemyAgent.ReachedDestinationOrGaveUp()) yield return new WaitForSecondsRealtime(Enemy.AISettings.PathRefreshInterval);
                Enemy.SetTask(new ReaquireEnemyTask(Enemy, EnemyAgent, _target));
                yield break;
            }

            yield return new WaitForSeconds(Enemy.AISettings.PathRefreshInterval);
        }
    }

    public override void OnSensorTriggered(AbstractEnemySensor sensor, object sensorResult) { }
}