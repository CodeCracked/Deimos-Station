using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomObjectiveManager : MonoBehaviour
{
    public Objective[] PossibleObjectives;
    public int ObjectiveCount;
    public Objective FinalObjective;

    public Objective[] Objectives { get; private set; }
    public int ObjectivesRemaining { get; private set; }

    public void Awake()
    {
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
        FinalObjective.enabled = false;
    }

    private void OnObjectiveCompleted()
    {
        ObjectivesRemaining--;
        if (ObjectivesRemaining <= 0) FinalObjective.enabled = true;
    }
    private void OnObjectiveReverted()
    {
        ObjectivesRemaining++;
        FinalObjective.enabled = false;
        FinalObjective.SetCompleted(false, true);
    }
}
