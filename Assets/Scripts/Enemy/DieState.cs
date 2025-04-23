using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieState : EnemyState
{
    public DieState(EnemyStateController controller) : base(controller) { }

    public override void EnterState()
    {
        Debug.Log("Entering Die State");
        stateController.NavAgent.isStopped = true; // Stop movement
        GameObject.Destroy(stateController.gameObject);
    }

    public override void UpdateState()
    {
        // No updates needed for the Die state
    }

    public override void ExitState()
    {
        //no logic needed
    }
}
