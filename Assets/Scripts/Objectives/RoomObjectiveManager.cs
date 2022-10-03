using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomObjectiveManager : MonoBehaviour
{
    public Objective[] PossibleObjectives;
    public int ObjectiveCount;

    public Objective[] Objectives { get; private set; }
    public int ObjectivesRemaining { get; private set; }

    public void Awake()
    {
        if (PossibleObjectives.Length == 0) PossibleObjectives = GetComponentsInChildren<Objective>();

        Objectives = new Objective[ObjectiveCount];
        List<Objective> randomized = new List<Objective>(PossibleObjectives).OrderBy(e => Guid.NewGuid()).ToList();
        foreach (Objective objective in PossibleObjectives) objective.gameObject.SetActive(false);
        for (int i = 0; i < ObjectiveCount; i++)
        {
            Objectives[i] = randomized[i];
            Objectives[i].OnCompleted.AddListener(OnObjectiveCompleted);
            Objectives[i].OnReverted.AddListener(OnObjectiveReverted);
            Objectives[i].gameObject.SetActive(true);
            if (!Objectives[i].Completed) ObjectivesRemaining++;
        }
    }

    private void OnObjectiveCompleted()
    {
        ObjectivesRemaining--;
    }
    private void OnObjectiveReverted()
    {
        ObjectivesRemaining++;
    }
}
