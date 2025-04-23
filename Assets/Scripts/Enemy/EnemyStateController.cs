using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateController : MonoBehaviour
{
    // Start is called before the first frame update
    public EnemyState CurrentState { get; private set; }
    public Transform Player;
    public float Health = 100f;
    public float DetectionRange = 15f;
    public float ShootingRange = 10f;

    public float RunSpeed = 3.5f;
    public Transform CoverPoint;

    [HideInInspector]
    public NavMeshAgent NavAgent;

    private void Start()
    {
        NavAgent = GetComponent<NavMeshAgent>();
        TransitionToState(new IdleState(this));
    }

    private void Update()
    {
        CurrentState?.UpdateState();
    }

    public void TransitionToState(EnemyState newState)
    {
        CurrentState?.ExitState();
        CurrentState = newState;
        CurrentState.EnterState();
    }

    public bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, Player.position) <= range;
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        Debug.Log($"Enemy took damage: {damage}. Current Health: {Health}");
        if (Health <= 0)
        {
            TransitionToState(new DieState(this));
        }

        if (Health <= 20f && CurrentState is not TakeCoverState)
        {
            TransitionToState(new TakeCoverState(this));
        }
    }

}
