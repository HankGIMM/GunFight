using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunState : EnemyState
{
    public RunState(EnemyStateController controller) : base(controller) { }

    public override void EnterState()
    {
        Debug.Log("Entering Run State");
        stateController.NavAgent.isStopped = false;
        stateController.NavAgent.speed = stateController.RunSpeed; // Set the speed to run speed
    }

    public override void UpdateState()
    {
        stateController.NavAgent.SetDestination(stateController.Player.position);

        if (stateController.IsPlayerInRange(stateController.ShootingRange))
        {
            stateController.TransitionToState(new ShootState(stateController));
        }
    }

    public override void ExitState()
    {
        Debug.Log("Exiting Run State");
    }
}
