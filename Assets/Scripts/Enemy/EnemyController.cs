using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    [Header("Enemy Config")]
    public ArtifactManager Artifact;
    public EnemySearchZone[] SearchZones;
    public EnemyAISettings AISettings;

    private NavMeshAgent _agent;
    private AbstractEnemyTask _currentTask;
    private Coroutine _currentTaskCoroutine;

    private AbstractEnemySensor[] _sensors;

    public void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

        _sensors = new AbstractEnemySensor[]
        {
            new VisionConeSensor(this, _agent, 0.1f),
        };

        SetTask(new SearchEnemyTask(this, _agent));
    }

    public void Update()
    {
        if (_currentTask != null && TestSensors(out AbstractEnemySensor sensor, out object result)) _currentTask.OnSensorTriggered(sensor, result);
    }

    public void SetTask(AbstractEnemyTask task)
    {
        if (_currentTask != null)
        {
            _currentTask.CancelTask();
            StopCoroutine(_currentTaskCoroutine);
        }

        _currentTask = task;
        _currentTaskCoroutine = StartCoroutine(task.RunTask());
    }
    public bool TestSensors(out AbstractEnemySensor sensor, out object sensorResult)
    {
        foreach (AbstractEnemySensor test in _sensors)
        {
            if (test.TestSensor(out sensorResult))
            {
                sensor = test;
                return true;
            }
        }

        sensor = null;
        sensorResult = null;
        return false;
    }
}

[Serializable]
public class EnemyAISettings
{
    [Header("Search Mode Settings")]
    public float LookAroundSpeed = 60.0f;
    public Vector2 LookTimerRange = new(1.0f, 3.0f);
    public float SearchSpeed = 3.0f;

    [Header("Vision Settings")]
    public float AreaSight = 16.0f;
    public float Blindsight = 3.0f;
}