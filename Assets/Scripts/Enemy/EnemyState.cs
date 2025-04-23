using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public abstract class EnemyState
{
    protected EnemyStateController stateController;

    public EnemyState(EnemyStateController controller)
    {
        stateController = controller;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
}
