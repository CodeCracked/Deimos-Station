using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    [Header("Enemy Config")]
    public EnemySearchZone[] SearchZones;
    public float SearchSpeed = 3.0f;

    [Header("Read Only - Debug Info")]
    public EnemyState State;

    private NavMeshAgent _agent;
    private AbstractEnemyTask _currentTask;
    private Coroutine _currentTaskCoroutine;

    public void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        SetState(EnemyState.Searching, new SearchEnemyTask(this, _agent));
    }

    public void SetState(EnemyState state, AbstractEnemyTask task)
    {
        if (_currentTask != null)
        {
            _currentTask.CancelTask();
            StopCoroutine(_currentTaskCoroutine);
        }

        _currentTask = task;
        _currentTaskCoroutine = StartCoroutine(task.RunTask());

        State = state;
    }
}

public enum EnemyState
{
    Searching,
}
