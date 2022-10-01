using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SearchEnemyTask : AbstractEnemyTask
{
    private static readonly int FailedPointThreshold = 10;
    private static readonly float PathCheckInterval = 0.2f;

    public SearchEnemyTask(EnemyController enemy, NavMeshAgent enemyAgent) : base(enemy, enemyAgent)
    {
    }

    public override IEnumerator RunTask()
    {
        EnemySearchZone searchZone = Enemy.SearchZones[Random.Range(0, Enemy.SearchZones.Length)];
        int searchLength = Random.Range(searchZone.SearchLengthMin, searchZone.SearchLengthMax + 1);
        NavMeshPath path = new();

        EnemyAgent.speed = Enemy.SearchSpeed;

        do
        {
            // Start New Search
            searchZone = Enemy.SearchZones[Random.Range(0, Enemy.SearchZones.Length)];
            searchLength = Random.Range(searchZone.SearchLengthMin, searchZone.SearchLengthMax + 1);

            Debug.LogFormat("Starting Search at {0} with length {1}", searchZone.gameObject.name, searchLength);


            // Run Search
            int failedPoints = 0;
            while (searchLength > 0 && failedPoints < FailedPointThreshold)
            {
                // Select Random Search Point and Calculate Path
                EnemySearchPoint searchPoint = searchZone.SearchPoints[Random.Range(0, searchZone.SearchPoints.Length)];
                EnemyAgent.CalculatePath(searchPoint.transform.position, path);

                // Check if Path is Valid
                if (path.status != NavMeshPathStatus.PathComplete)
                {
                    failedPoints++;
                    continue;
                }
                else
                {
                    failedPoints = 0;
                    EnemyAgent.SetPath(path);
                }

                // Wait for Path Completion
                while (!EnemyAgent.ReachedDestinationOrGaveUp()) yield return new WaitForSecondsRealtime(PathCheckInterval);
                searchLength--;

                Debug.LogFormat("Reached target point. Remaining points: {0}", searchLength);
            }

            Debug.Log("Finished Search", Enemy);
        }
        while (!Finished);
    }
}