using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TakeCoverState : EnemyState
{
    public TakeCoverState(EnemyStateController controller) : base(controller) { }

    public override void EnterState()
    {
        Debug.Log("Entering Take Cover State");

        // Find all cover points in the scene
        GameObject[] coverPoints = GameObject.FindGameObjectsWithTag("CoverPoint");

        if (coverPoints.Length == 0)
        {
            Debug.LogError("No cover points found in the scene!");
            return;
        }

        // Find the closest cover point
        Transform closestCoverPoint = FindClosestCoverPoint(coverPoints);

        if (closestCoverPoint == null)
        {
            Debug.LogError("No valid cover point found!");
            return;
        }

        stateController.CoverPoint = closestCoverPoint;

        // Validate the cover point on the NavMesh
        if (!UnityEngine.AI.NavMesh.SamplePosition(stateController.CoverPoint.position, out UnityEngine.AI.NavMeshHit hit, 5f, UnityEngine.AI.NavMesh.AllAreas))
        {
            Debug.LogError("Cover point is not valid. Cannot enter Take Cover state.");
            return;
        }

        stateController.NavAgent.isStopped = false;
        stateController.NavAgent.SetDestination(stateController.CoverPoint.position);
    }

    public override void UpdateState()
    {
        if (Vector3.Distance(stateController.transform.position, stateController.CoverPoint.position) < 1f)
        {
            stateController.TransitionToState(new IdleState(stateController));
        }
    }

    public override void ExitState()
    {
        Debug.Log("Exiting Take Cover State");
    }

    private Transform FindClosestCoverPoint(GameObject[] coverPoints)
    {
        Transform closestPoint = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject coverPoint in coverPoints)
        {
            float distance = Vector3.Distance(stateController.transform.position, coverPoint.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPoint = coverPoint.transform;
            }
        }

        return closestPoint;
    }
}
