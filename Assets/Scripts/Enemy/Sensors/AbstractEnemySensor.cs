using UnityEngine;
using UnityEngine.AI;

public abstract class AbstractEnemySensor
{
    protected readonly EnemyController Enemy;
    protected readonly NavMeshAgent EnemyAgent;
    protected readonly float TestInterval;

    private float _timeoutTime;

    public AbstractEnemySensor(EnemyController enemy, NavMeshAgent enemyAgent, float testInterval)
    {
        Enemy = enemy;
        EnemyAgent = enemyAgent;
    }

    public void DisableSensor(float duration)
    {
        _timeoutTime = Mathf.Max(_timeoutTime, Time.time + duration);
    }
    public void EnableSensor()
    {
        _timeoutTime = 0;
    }
    public bool TestSensor(out object sensorResult)
    {
        if (Time.time < _timeoutTime)
        {
            sensorResult = null;
            return false;
        }
        else
        {
            DisableSensor(TestInterval);
            return ExecuteSensor(out sensorResult);
        }
    }

    protected abstract bool ExecuteSensor(out object sensorResult);
}
