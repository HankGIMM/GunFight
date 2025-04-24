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

        // Delay destruction to avoid conflicts with other states
        stateController.StartCoroutine(DestroyAfterDelay(0.4f)); // Small delay to ensure no further logic runs
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        WaveSpawner waveSpawner = Object.FindObjectOfType<WaveSpawner>();
        if (waveSpawner != null)
        {
            waveSpawner.RemoveEnemy(stateController.gameObject);
        }

        GameObject.Destroy(stateController.gameObject.transform.root.gameObject); // Destroy the enemy object
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
