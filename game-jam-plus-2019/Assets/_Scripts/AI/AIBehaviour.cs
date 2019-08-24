using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBehaviour : MonoBehaviour
{
    public CheckpointManager CheckpointManager;
    public GameObject Flag;
    public float speed;
    public float boost;

    private Checkpoint target;

    private void Start()
    {
        CheckpointManager = FindObjectOfType<CheckpointManager>();
        InvokeRepeating("GetTargetCheckpoint", 0, 0.5f);
    }

    private void Update()
    {
        if (target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position,
                (speed + boost) * Time.deltaTime);

            if (Vector3.Distance(transform.position, target.transform.position) < 0.1f)
            {
                PlantFlag();
                target = null;
            }
        }
    }

    private void GetTargetCheckpoint()
    {
        List<Checkpoint> targets = CheckpointManager.AvailableTargets();
        float shortestDistance = Mathf.Infinity;
        Checkpoint nearestTarget = null;

        foreach (Checkpoint checkpoint in targets)
        {
            float distanceToTarget = Vector3.Distance(transform.position, checkpoint.transform.position);
            if (distanceToTarget < shortestDistance)
            {
                shortestDistance = distanceToTarget;
                nearestTarget = checkpoint;
            }
        }

        if (nearestTarget != null)
        {
            target = nearestTarget;
        }
    }

    private void PlantFlag()
    {
        Instantiate(Flag, transform.position + transform.forward, Quaternion.identity);
        target.GetComponent<Checkpoint>().Capture();
        Destroy(gameObject);
    }
}
