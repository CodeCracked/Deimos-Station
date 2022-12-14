using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SearchEnemyTask : AbstractEnemyTask
{
    private static readonly int FailedPointThreshold = 10;

    public SearchEnemyTask(EnemyController enemy, NavMeshAgent enemyAgent) : base(enemy, enemyAgent)
    {
    }

    #region Task Actions
    public override IEnumerator RunTask()
    {
        EnemySearchZone searchZone;
        int searchLength;
        NavMeshPath path = new();

        EnemyAgent.speed = Enemy.AISettings.SearchSpeed;

        do
        {
            // Start New Search
            searchZone = SearchZoneManager.BeginPrioritySearch(Enemy.CurrentZone, Enemy.PermittedZones);
            searchLength = Random.Range(searchZone.SearchLengthMin, searchZone.SearchLengthMax + 1);

            Debug.LogFormat("Starting Search at {0} with length {1}", searchZone.gameObject.name, searchLength);

            // Run Search
            int failedPoints = 0;
            while (searchLength > 0 && failedPoints < FailedPointThreshold)
            {
                // Select Random Search Point and Calculate Path
                EnemySearchPoint searchPoint;
                do { searchPoint = searchZone.SearchPoints[Random.Range(0, searchZone.SearchPoints.Length)]; }
                while (!searchPoint.gameObject.activeSelf);
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
                while (!EnemyAgent.ReachedDestinationOrGaveUp()) yield return new WaitForSecondsRealtime(Enemy.AISettings.PathRefreshInterval);
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
        SearchArc lastSearchArc = null;

        Enemy.Artifact.State = ArtifactState.Focused;
        while (Time.time < lingerDoneAt)
        {
            SearchArc arc;
            do { arc = point.SearchArcs[Random.Range(0, point.SearchArcs.Length)]; }
            while (point.SearchArcs.Length > 1 && arc == lastSearchArc);

            yield return LookAtAngle(Random.Range(arc.AngleMinimum, arc.AngleMaximum), Enemy.AISettings.SearchLookSpeed);
            yield return new WaitForSeconds(Random.Range(Enemy.AISettings.LookTimerRange.x, Enemy.AISettings.LookTimerRange.y));
        }
        Enemy.Artifact.State = ArtifactState.Area;
    }
    #endregion
    #region Sensor Actions
    public override void OnSensorTriggered(AbstractEnemySensor sensor, object sensorResult)
    {
        if (sensor is VisionConeSensor && sensorResult is VisionConeHit visionHit)
        {
            if (Enemy.Artifact.State == ArtifactState.Focused)
            {
                Enemy.SoundManager.PlaySound(Enemy.NoticedSound, 96.0f, 1.0f);
                Enemy.SetTask(new PursueEnemyTask(Enemy, EnemyAgent, visionHit.Target));
            }
            else
            {
                Enemy.SetTask(new InvestigateEnemyTask(Enemy, EnemyAgent, visionHit.Target.transform.position));
                sensor.DisableSensor(1.0f);
            }
        }
        else if (sensor is ArtifactFocusSensor && sensorResult is VisionConeTarget visionTarget) Enemy.SetTask(new InvestigateEnemyTask(Enemy, EnemyAgent, visionTarget.transform.position));
    }
    #endregion
}