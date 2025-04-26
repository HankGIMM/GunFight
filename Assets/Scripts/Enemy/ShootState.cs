using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShootState : EnemyState
{
    private float shootCooldown = 1f;
    private float lastShootTime;

    private EnemyGun enemyGun; // Reference to the enemy's gun

    public ShootState(EnemyStateController controller) : base(controller)
    {
        enemyGun = controller.GetComponentInChildren<EnemyGun>(); // Get the enemy's gun component
        if (enemyGun == null)
        {
            Debug.LogError("EnemyGun component not found on the enemy.");
        }

    }
    public override void EnterState()
    {
        Debug.Log("Entering Shoot State");
        stateController.NavAgent.isStopped = true; // Stop movement
    }

    public override void UpdateState()
    {
        // Check if the player is still in range to shoot
        if (!stateController.IsPlayerInRange(stateController.ShootingRange))
        {
            stateController.TransitionToState(new RunState(stateController)); // Transition back to Run state
            return;
        }

        if (Time.time - lastShootTime >= shootCooldown)
        {
            if (enemyGun != null)
            {
                enemyGun.Shoot();
                lastShootTime = Time.time;
            }
        }

        if (stateController.Health <= 30f)
        {
            stateController.TransitionToState(new TakeCoverState(stateController));
        }


    }

    private void Shoot()
    {
        Debug.Log("Enemy shoots at the player!");

    }

    public override void ExitState()
    {
        Debug.Log("Exiting Shoot State");
        // stateController.NavAgent.isStopped = false; // Resume movement
        //  stateController.TransitionToState(new RunState(stateController)); // Transition back to Run state

    }
}
