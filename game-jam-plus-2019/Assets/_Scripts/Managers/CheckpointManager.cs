using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public List<Checkpoint> Checkpoints;

    public List<Checkpoint> AvailableTargets()
    {
        List<Checkpoint> uncapturedCheckpoints = new List<Checkpoint>();

        foreach (Checkpoint checkpoint in Checkpoints)
        {
            if(!checkpoint.IsCaptured)
                uncapturedCheckpoints.Add(checkpoint);
        }

        return uncapturedCheckpoints;
    }
}
