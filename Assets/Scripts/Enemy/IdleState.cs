using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : EnemyState
{
    public IdleState(EnemyStateController controller) : base(controller) { }

    public override void EnterState()
    {
        Debug.Log("Entering Idle State");
        stateController.NavAgent.isStopped = true; // Stop movement
    }

    public override void UpdateState()
    {
        if (stateController.IsPlayerInRange(stateController.DetectionRange))
        {
            stateController.TransitionToState(new RunState(stateController));
        }
    }

    public override void ExitState()
    {
        Debug.Log("Exiting Idle State");
    }
}
