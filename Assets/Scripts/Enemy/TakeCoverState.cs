using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeCoverState : EnemyState
{
    public TakeCoverState(EnemyStateController controller) : base(controller) { }

    public override void EnterState()
    {
        Debug.Log("Entering Take Cover State");

        if (stateController.CoverPoint == null)
        {
            Debug.LogError("Cover point is not set. Cannot enter Take Cover state.");
            return;
        }

        if (!UnityEngine.AI.NavMesh.SamplePosition(stateController.CoverPoint.position, out UnityEngine.AI.NavMeshHit hit, 1f, UnityEngine.AI.NavMesh.AllAreas))
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
}
