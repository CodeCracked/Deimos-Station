using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class AbstractEnemyTask
{
    protected readonly EnemyController Enemy;
    protected readonly NavMeshAgent EnemyAgent;
    public bool Finished { get; protected set; }

    public AbstractEnemyTask(EnemyController enemy, NavMeshAgent enemyAgent)
    {
        Enemy = enemy;
        EnemyAgent = enemyAgent;
    }

    public abstract IEnumerator RunTask();
    public abstract void OnSensorTriggered(AbstractEnemySensor sensor, object sensorResult);
    public virtual void CancelTask() { }

    #region Helpers
    protected IEnumerator LookAtAngle(float targetAngle, float lookSpeed)
    {
        targetAngle = Mathf.Repeat(targetAngle, 360.0f);
        float targetDelta = Mathf.DeltaAngle(Enemy.transform.rotation.eulerAngles.y, targetAngle);

        float deltaSign = Mathf.Sign(targetDelta);
        float step = deltaSign * lookSpeed;
        float deltaLeft = targetDelta;

        do
        {
            float delta = step * Time.deltaTime;
            deltaLeft -= delta;
            Enemy.transform.Rotate(0, delta, 0);
            yield return null;
        }
        while (deltaSign == Mathf.Sign(deltaLeft));
    }
    protected IEnumerator CancellableWait(float delay, Func<bool> cancelCheck = null)
    {
        if (cancelCheck == null) yield return new WaitForSeconds(delay);
        else
        {
            float finishedTime = Time.time + delay;
            while (Time.time < finishedTime)
            {
                if (cancelCheck()) yield break;
                else yield return null;
            }
        }
    }
    #endregion
}