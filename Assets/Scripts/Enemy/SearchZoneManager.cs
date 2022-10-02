using System.Collections.Generic;
using UnityEngine;

public static class SearchZoneManager
{
    private static readonly Dictionary<EnemySearchZone, SearchStatus> _zones = new();

    public static void RegisterZone(EnemySearchZone zone)
    {
        _zones[zone] = new SearchStatus();
    }
    public static SearchStatus GetStatus(EnemySearchZone zone)
    {
        return _zones[zone];
    }
    public static void BeginSearch(EnemySearchZone zone)
    {
        SearchStatus status = _zones[zone];
        status.SearchInProgress = true;
        status.SearchStartTime = Time.time;
    }
    public static void EndSearch(EnemySearchZone zone)
    {
        SearchStatus status = _zones[zone];
        status.SearchInProgress = false;
        status.SearchEndTime = Time.time;
    }

    public static EnemySearchZone BeginPrioritySearch(EnemySearchZone currentZone, List<EnemySearchZone> permittedZones)
    {
        EnemySearchZone currentDecision = null;
        SearchStatus currentDecisionStatus = null;

        if (permittedZones.Count == 1)
        {
            currentDecision = permittedZones[0];
            BeginSearch(currentDecision);
            return currentDecision;
        }

        foreach (EnemySearchZone zone in currentZone.ConnectedZones)
        {
            // If this zone is not permitted, continue to next zone
            if (!permittedZones.Contains(zone)) continue;

            SearchStatus status = _zones[zone];

            // If there is no desicion yet, select this zone
            if (!currentDecision)
            {
                currentDecision = zone;
                currentDecisionStatus = status;
                continue;
            }

            // If the current desicion has a search in progress
            else if (currentDecisionStatus.SearchInProgress)
            {
                // If this zone has no search, or this zone's search is older than the current desicion, select this zone
                if (!status.SearchInProgress || status.SearchStartTime < currentDecisionStatus.SearchStartTime)
                {
                    currentDecision = zone;
                    currentDecisionStatus = status;
                    continue;
                }
            }

            // If the current desicion has no search
            else
            {
                // If this zone has no search and has had a longer time since the last search, select this zone
                if (!status.SearchInProgress && status.SearchEndTime > currentDecisionStatus.SearchEndTime)
                {
                    currentDecision = zone;
                    currentDecisionStatus = status;
                    continue;
                }
            }
        }

        if (!currentDecision) currentDecision = permittedZones[Random.Range(0, permittedZones.Count)];
        BeginSearch(currentDecision);
        return currentDecision;
    }

    public class SearchStatus
    {
        public bool SearchInProgress = false;
        public float SearchStartTime = Random.value;
        public float SearchEndTime = Random.value;
    }
}
