using System;
using UnityEngine;

public class ArtifactController : MonoBehaviour
{
    public PlayerController Player;
    public ArtifactManager Artifact;
    public string FocusButton = "Fire1";
    public string ConcealButton = "Fire2";

    public void Awake()
    {
        Artifact.OnStateChange.AddListener(OnArtifactStateChanged);
    }

    private void OnArtifactStateChanged(ArtifactState newState)
    {
        if (newState == ArtifactState.Focused) Player.Speed *= 0.5f;
        else if (Artifact.State == ArtifactState.Focused) Player.Speed *= 2;
    }

    public void Update()
    {
        if (Input.GetButton(ConcealButton)) Artifact.State = ArtifactState.Concealed;
        else if (Input.GetButton(FocusButton)) Artifact.State = ArtifactState.Focused;
        else Artifact.State = ArtifactState.Area;
    }
}
