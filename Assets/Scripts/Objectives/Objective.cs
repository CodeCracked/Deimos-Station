using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Objective : MonoBehaviour
{
    [Header("Alert Settings")]
    public PlayerController Player;
    [Range(0.0f, 1.0f)] public float AlertChance = 0.0f;
    public float AlertDistance = 64.0f;
    public LayerMask EnemyLayerMask;

    [Header("Objective Settings")]
    public bool Completed = false;
    public bool CanRevert = false;
    [ColorUsage(true, true)] public Color StartingColor = 2 * Color.red;
    [ColorUsage(true, true)] public Color MetColor = 2 * Color.green;
    public MeshRenderer Renderer;
    public UnityEvent OnCompleted;
    public UnityEvent OnReverted;

    private bool _glowing;

    public void Start()
    {
        if (OptionsManager.GlowingObjectives)
        {
            Renderer.material.EnableKeyword("_EMISSION");
            _glowing = true;
        }
        else
        {
            Renderer.material.DisableKeyword("_EMISSION");
            Renderer.material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack;
            _glowing = false;
        }

        Renderer.material.SetColor("_EmissionColor", Completed ? MetColor : StartingColor);
        Renderer.material.color = Completed ? MetColor : StartingColor;
        OnCompleted.AddListener(DoAlertCheck);
    }
    public void Update()
    {
        if (OptionsManager.GlowingObjectives != _glowing)
        {
            if (OptionsManager.GlowingObjectives)
            {
                Renderer.material.EnableKeyword("_EMISSION");
                _glowing = true;
            }
            else
            {
                Renderer.material.DisableKeyword("_EMISSION");
                Renderer.material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack;
                _glowing = false;
            }
        }
    }

    public void SetCompleted(bool completed = true, bool force = false)
    {
        if (!enabled && !force) return;
        if (Completed && !completed && !CanRevert && !force) return;
        if (Completed != completed)
        {
            if (completed) OnCompleted.Invoke();
            else OnReverted.Invoke();
            Completed = completed;
        }
        Renderer.material.SetColor("_EmissionColor", Completed ? MetColor : StartingColor);
        Renderer.material.color = Completed ? MetColor : StartingColor;
    }
    public void Toggle()
    {
        SetCompleted(!Completed);
    }

    private void DoAlertCheck()
    {
        if (Random.value < AlertChance)
        {
            Collider[] overlap = Physics.OverlapSphere(Player.transform.position, AlertDistance, EnemyLayerMask);
            EnemyController alertedEnemy = null;
            float alertedDistance = int.MaxValue;

            // For Each Collider in Range
            foreach (Collider collider in overlap)
            {
                EnemyController enemy = collider.GetComponent<EnemyController>();
                NavMeshAgent enemyAgent = collider.GetComponent<NavMeshAgent>();

                // If the collider is an enemy
                if (enemy && enemyAgent)
                {
                    // Calculate a path from the enemy to the alert
                    NavMeshPath path = new();
                    enemyAgent.CalculatePath(Player.transform.position, path);

                    // If the path exists
                    if (path.status == NavMeshPathStatus.PathComplete)
                    {
                        // Check the path distance isn't more than the alert distance and that the enemy is currently searching
                        float distance = path.GetLength();
                        if (distance > AlertDistance) continue;
                        if (enemy.CurrentTask is not SearchEnemyTask) continue;

                        // If the enemy is the closest, select this enemy
                        if (!alertedEnemy || alertedDistance > distance)
                        {
                            alertedEnemy = enemy;
                            alertedDistance = distance;
                        }
                    }
                }
            }

            // Alert the enemy if one was selected
            if (alertedEnemy) alertedEnemy.SetTask(new InvestigateEnemyTask(alertedEnemy, alertedEnemy.GetComponent<NavMeshAgent>(), Player.transform.position));
        }
    }
}
