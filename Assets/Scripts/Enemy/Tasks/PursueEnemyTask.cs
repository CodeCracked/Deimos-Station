using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PursueEnemyTask : AbstractEnemyTask
{
    private readonly VisionConeTarget _target;

    private Vector3 _lastSpottedPosition;
    private float _targetLastSpotted;

    public PursueEnemyTask(EnemyController enemy, NavMeshAgent enemyAgent, VisionConeTarget target) : base(enemy, enemyAgent)
    {
        _target = target;
    }

    public override IEnumerator RunTask()
    {
        Enemy.SoundManager.PlaySound(Enemy.PursueSound, 96.0f, 1.0f);
        Enemy.Artifact.State = ArtifactState.Focused;

        NavMeshPath path = new();
        _lastSpottedPosition = _target.transform.position;
        _targetLastSpotted = Time.time;

        EnemyAgent.speed = Enemy.AISettings.PersueSpeed;
        EnemyAgent.CalculatePath(_lastSpottedPosition, path);
        EnemyAgent.SetPath(path);

        while (true)
        {
            // Look for Player
            if (Enemy.VisionCone.CanSeeTarget(_target))
            {
                EnemyAgent.CalculatePath(_lastSpottedPosition, path);
                EnemyAgent.SetPath(path);
                _lastSpottedPosition = _target.transform.position;
                _targetLastSpotted = Time.time;
            }
            else
            {

            }

            // Check Timeout
            if (Time.time - _targetLastSpotted > Enemy.AISettings.MaxTimeWithoutSpotting)
            {
                Enemy.SetTask(new SearchEnemyTask(Enemy, EnemyAgent));
                break;
            }

            yield return new WaitForSeconds(Enemy.AISettings.PathRefreshInterval);
        }
    }

    public override void OnSensorTriggered(AbstractEnemySensor sensor, object sensorResult) { }
}