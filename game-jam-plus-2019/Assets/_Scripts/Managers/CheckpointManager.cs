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

    public void Restart()
    {
        foreach (Checkpoint checkpoint in Checkpoints)
        {
            checkpoint.IsCaptured = false;
            Destroy(checkpoint.transform.Find("Flag Waving(Clone)").gameObject);
        }
    }
}
