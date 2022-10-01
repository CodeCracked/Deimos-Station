using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SearchEnemyTask : AbstractEnemyTask
{
    private static readonly int FailedPointThreshold = 10;
    private static readonly float PathCheckInterval = 0.2f;
    private static readonly float LookAroundSpeed = 60.0f;
    private static readonly Vector2 LookTimerRange = new(1.0f, 3.0f);

    public SearchEnemyTask(EnemyController enemy, NavMeshAgent enemyAgent) : base(enemy, enemyAgent)
    {
    }

    public override IEnumerator RunTask()
    {
        EnemySearchZone searchZone;
        int searchLength;
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

                // Wait for Path Completion, Then Look Around
                while (!EnemyAgent.ReachedDestinationOrGaveUp()) yield return new WaitForSecondsRealtime(PathCheckInterval);
                yield return LookAroundPoint(searchPoint);
                searchLength--;

                Debug.LogFormat("Reached target point. Remaining points: {0}", searchLength);
            }

            Debug.Log("Finished Search", Enemy);
        }
        while (!Finished);
    }

    private IEnumerator LookAroundPoint(EnemySearchPoint point)
    {
        float lingerDoneAt = Time.time + Random.Range(point.LingerTimeMin, point.LingerTimeMax);

        Enemy.Artifact.State = ArtifactState.Focused;
        while (Time.time < lingerDoneAt)
        {
            yield return LookToAngle(Random.insideUnitCircle.normalized);
            yield return new WaitForSeconds(Random.Range(LookTimerRange.x, LookTimerRange.y));
        }
        Enemy.Artifact.State = ArtifactState.Area;
    }

    private IEnumerator LookToAngle(Vector2 targetHeading)
    {
        Vector2 forward = new Vector2(Enemy.transform.forward.x, Enemy.transform.forward.z).normalized;
        float targetDelta = Vector2.SignedAngle(forward, targetHeading);
        float deltaSign = Mathf.Sign(targetDelta);
        float step = deltaSign * LookAroundSpeed;
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
}