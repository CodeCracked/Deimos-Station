using System.Collections;
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
    public virtual void CancelTask() { }
}