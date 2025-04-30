using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;

public abstract class Bullet : MonoBehaviour
{
    public float speed;
    public float damage;
    public Rigidbody rb;
    public GameObject impactEffect;
    public GameObject bloodEffect;

    private EnemyStateController enemyStateController;
    private PlayerController playerController;


    public bool isEnemyBullet = false; // Flag to differentiate between player and enemy bullets


    public float gravity = 9.81f;
    public float drag = 0.01f;

    public abstract void Initialize(Vector3 direction);

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; //gravity handled elsewhere 
        StartCoroutine(ApplyPhysics());
        StartCoroutine(DespawnAfterTime(3.0f));
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Bullet collided with: {collision.gameObject.name}, Tag: {collision.gameObject.tag}");


        if (collision.gameObject.CompareTag("Enemy"))
        {
            HandleEnemyCollision(collision);
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            HandleWallCollision(collision);
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            HandleGroundCollision(collision);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            HandleCollision(collision); // Handle player collision
        }
        // else if (collision.gameObject.CompareTag("Target"))
        // {
        //     HandleCollision(collision); // Handle target collision
        // }
        else
        {
            HandleCollision(collision); // Default behavior for other tags
        }
    }

    private void HandleEnemyCollision(Collision collision)
    {
        // Apply damage to the enemy
        EnemyStateController enemy = collision.gameObject.GetComponent<EnemyStateController>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Debug.Log($"Hit enemy: {collision.gameObject.name}, Damage: {damage}");
        }

        // Instantiate blood effect
        if (bloodEffect != null)
        {
            Instantiate(bloodEffect, collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal));
        }

        // Play ricochet sound
        PlayRicochetSound(collision.contacts[0].point, collision.gameObject.tag);
        // Destroy the bullet
        Destroy(gameObject);
    }

    private void HandleWallCollision(Collision collision) // Energy Based Richochet updated for wall
    {
        // Calculate the angle of impact
        float impactAngle = Vector3.Angle(collision.contacts[0].normal, -rb.velocity.normalized);

        // Only ricochet if the angle is shallow enough
        if (impactAngle < 45f)
        {
            Vector3 ricochetDirection = Vector3.Reflect(rb.velocity.normalized, collision.contacts[0].normal);
            rb.velocity = ricochetDirection * (rb.velocity.magnitude * 0.5f); // Reduce speed after ricochet
        }
        else
        {
            // Stop the bullet if the angle is too steep
            rb.velocity = Vector3.zero;
            rb.useGravity = true; // Enable gravity to make the bullet fall
        }

        // Play ricochet sound
        PlayRicochetSound(collision.contacts[0].point, collision.gameObject.tag);
        // Optional: Play impact effect
        if (impactEffect != null)
        {
            Instantiate(impactEffect, collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal));
        }



    }

    private void HandleGroundCollision(Collision collision) //Energy Based Richochet - 
    {
        // Calculate the angle of impact
        float impactAngle = Vector3.Angle(collision.contacts[0].normal, -rb.velocity.normalized);

        // Only ricochet if the angle is shallow enough
        if (impactAngle < 45f)
        {
            Vector3 ricochetDirection = Vector3.Reflect(rb.velocity.normalized, collision.contacts[0].normal);
            rb.velocity = ricochetDirection * (rb.velocity.magnitude * 0.5f); // Reduce speed after ricochet
        }
        else
        {
            // Stop the bullet if the angle is too steep
            rb.velocity = Vector3.zero;
            rb.useGravity = true;
        }

        // Play ricochet sound
        PlayRicochetSound(collision.contacts[0].point, collision.gameObject.tag);

        // Optional: Play impact effect
        if (impactEffect != null)
        {
            Instantiate(impactEffect, collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal));
        }
    }

    protected void HandlePlayerCollision(Collision collision)
    {
        // Apply damage to the player
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            player.TakeDamage(damage);
            Debug.Log($"Hit player: {collision.gameObject.name}, Damage: {damage}");
        }

        //play ricochet sound
        PlayRicochetSound(collision.contacts[0].point, collision.gameObject.tag);

        // Instantiate blood effect
        // if (bloodEffect != null)
        // {
        //     Instantiate(bloodEffect, collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal));
        // }

        // Destroy the bullet
        Destroy(gameObject);
    }


    protected void HandleCollision(Collision collision)
    {
        // Default collision behavior (e.g., for untagged objects)
        if (impactEffect != null)
        {
            Instantiate(impactEffect, collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal));
        }

        // Play sound based on tag
        PlayRicochetSound(collision.contacts[0].point, collision.gameObject.tag);
    }

    private IEnumerator ApplyPhysics()
    {
        while (true)
        {
            // Apply gravity
            Vector3 gravityForce = Vector3.down * gravity * Time.fixedDeltaTime;
            rb.velocity += gravityForce;

            // Apply drag
            Vector3 dragForce = rb.velocity.normalized * (rb.velocity.magnitude * drag);
            rb.velocity -= dragForce * Time.fixedDeltaTime;

            // Debug.Log($"Bullet velocity: {rb.velocity.magnitude}");

            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator DespawnAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    private void PlayRicochetSound(Vector3 position, string tag)
    {
        // Delegate ricochet sound playback to the AudioManager
        AudioManager.Instance.PlayRicochetSound(position, tag);
    }
}