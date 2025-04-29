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

    public EnemyGun enemyGun; // Reference to the enemy's gun



    [HideInInspector]
    public NavMeshAgent NavAgent;

    private bool isDead = false; // check if the enemy is dead

    private void Start()
    {
        if (enemyGun == null)
        {
            enemyGun = GetComponent<EnemyGun>(); // Get the enemy's gun component
            if (enemyGun == null)
            {
                Debug.LogError("EnemyGun component not found on the enemy.");
            }
        }
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        if (Player == null)
        {
            Debug.LogError("Player not found. Make sure the player has the tag 'Player'.");
            return;
        }
        NavAgent = GetComponent<NavMeshAgent>();
        TransitionToState(new IdleState(this));
    }

    private void Update()
    {
        if (isDead) return; // Prevent updates if already dead
        CurrentState?.UpdateState();
    }

    public void FacePlayer()
    {
        if (Player == null) return;

        // Calculate the direction to the player
        Vector3 directionToPlayer = Player.position - transform.position;
        directionToPlayer.y = 0; // Keep the rotation on the horizontal plane

        // Rotate the enemy to face the player
        if (directionToPlayer != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f); // Smooth rotation
        }
    }

    public void TransitionToState(EnemyState newState)
    {
        if (this == null || gameObject == null) // Check if the enemy is destroyed
        {
            Debug.LogWarning("Enemy is already destroyed. Cannot transition to a new state.");
            return;
        }

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
        if (isDead) return; // Prevent taking damage if already dead


        Health -= damage;
        Debug.Log($"Enemy took damage: {damage}. Current Health: {Health}");
        if (Health <= 0)
        {
            isDead = true;
            TransitionToState(new DieState(this));
        }

        if (Health <= 20f && CurrentState is not TakeCoverState)
        {
            TransitionToState(new TakeCoverState(this));
        }
    }

}
