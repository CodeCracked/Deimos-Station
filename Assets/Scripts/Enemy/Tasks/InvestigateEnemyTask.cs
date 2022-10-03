using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class InvestigateEnemyTask : AbstractEnemyTask
{
    private readonly Vector3 _target;

    public InvestigateEnemyTask(EnemyController enemy, NavMeshAgent enemyAgent, Vector3 target) : base(enemy, enemyAgent)
    {
        _target = target;
    }

    public override IEnumerator RunTask()
    {
        Enemy.Artifact.State = ArtifactState.Focused;
        Enemy.SoundManager.PlaySound(Enemy.InvestigateSound, 96.0f, 1.0f);

        NavMeshPath path = new();
        EnemyAgent.speed = Enemy.AISettings.InvestigateSpeed;
        EnemyAgent.CalculatePath(_target, path);
        EnemyAgent.SetPath(path);

        while (true)
        {
            while (!EnemyAgent.ReachedDestinationOrGaveUp()) yield return new WaitForSecondsRealtime(Enemy.AISettings.PathRefreshInterval);
            yield return LookAroundForTarget();
            yield return SearchForTarget();
            Enemy.SetTask(new SearchEnemyTask(Enemy, EnemyAgent));
        }
    }

    private IEnumerator LookAroundForTarget()
    {
        // Do two 180-degree look arcs
        float startingAngle = Enemy.transform.rotation.eulerAngles.y;
        if (Random.value < 0.5f)
        {
            yield return LookAtAngle(startingAngle + 179, Enemy.AISettings.ReaquireLookSpeed);
            yield return new WaitForSeconds(0.5f);
            yield return LookAtAngle(startingAngle, Enemy.AISettings.ReaquireLookSpeed);
            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            yield return LookAtAngle(startingAngle - 179, Enemy.AISettings.ReaquireLookSpeed);
            yield return new WaitForSeconds(0.5f);
            yield return LookAtAngle(startingAngle, Enemy.AISettings.ReaquireLookSpeed);
            yield return new WaitForSeconds(0.5f);
        }

        // Resume Search Mode
        Enemy.SetTask(new SearchEnemyTask(Enemy, EnemyAgent));
    }
    private IEnumerator SearchForTarget()
    {
        yield break;
    }

    public override void OnSensorTriggered(AbstractEnemySensor sensor, object sensorResult)
    {
        if (sensor is VisionConeSensor && sensorResult is VisionConeHit visionHit)
        {
            Enemy.SoundManager.PlaySound(Enemy.NoticedSound, 96.0f, 1.0f);
            Enemy.SetTask(new PursueEnemyTask(Enemy, EnemyAgent, visionHit.Target));
        }
        else if (sensor is ArtifactFocusSensor && sensorResult is VisionConeTarget visionTarget)
        {
            Enemy.SoundManager.PlaySound(Enemy.NoticedSound, 96.0f, 1.0f);
            Enemy.SetTask(new PursueEnemyTask(Enemy, EnemyAgent, visionTarget));
        }
    }
}