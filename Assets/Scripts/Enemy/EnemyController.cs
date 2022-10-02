using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    [Header("Enemy Config")]
    public ArtifactManager Artifact;
    public EnemySoundManager SoundManager;
    public VisionCone VisionCone;
    public EnemySearchZone CurrentZone;
    public List<EnemySearchZone> PermittedZones;
    public EnemyAISettings AISettings;

    [Header("Enemy Sounds")]
    public AudioClipPool NoticedSound;

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
            new ArtifactFocusSensor(this, _agent, 0.1f)
        };
    }
    public void Start()
    {
        SetTask(new SearchEnemyTask(this, _agent));
    }

    public void Update()
    {
        if (_currentTask != null && TestSensors(out AbstractEnemySensor sensor, out object result)) _currentTask.OnSensorTriggered(sensor, result);
    }

    public void SetTask(AbstractEnemyTask task)
    {
        if (task == null) Debug.Log(gameObject.name + " is stopping tasks", this);
        else Debug.Log(gameObject.name + " is starting task: " + task.GetType().Name, this);

        if (_currentTask != null)
        {
            _currentTask.CancelTask();
            StopCoroutine(_currentTaskCoroutine);
        }

        _agent.ResetPath();
        _currentTask = task;
        _currentTaskCoroutine = task != null ? StartCoroutine(task.RunTask()) : null;
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
    [Header("Generic Settings")]
    public float PathRefreshInterval = 0.2f;

    [Header("Search Mode Settings")]
    public float SearchSpeed = 3.0f;
    public float SearchLookSpeed = 60.0f;
    public Vector2 LookTimerRange = new(1.0f, 3.0f);

    [Header("Persue Settings")]
    public float PersueSpeed = 6.0f;
    public float MaxTimeWithoutSpotting = 7.5f;
    public float ReaquireLookSpeed = 360.0f;

    [Header("Vision Settings")]
    public float AreaSight = 16.0f;
    public float Blindsight = 3.0f;
    public float SenseFocusedArtifactRange = 32.0f;
}