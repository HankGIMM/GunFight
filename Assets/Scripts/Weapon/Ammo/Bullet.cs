using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    public Dictionary<string, AudioClip> tagToAudioClip = new Dictionary<string, AudioClip>();
    public abstract void Initialize(Vector3 direction);

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; //gravity handled elsewhere 
        StartCoroutine(ApplyPhysics());
        StartCoroutine(DespawnAfterTime(3.0f));

        tagToAudioClip.Add("Wall", Resources.Load<AudioClip>("Assets/Audio/SFX/RicochetWall"));
        tagToAudioClip.Add("Target", Resources.Load<AudioClip>("Assets/Audio/SFX/RicochetTarget"));
        tagToAudioClip.Add("Ground", Resources.Load<AudioClip>("Assets/Audio/SFX/RicochetGround"));
        tagToAudioClip.Add("Enemy", Resources.Load<AudioClip>("Assets/Audio/SFX/RicochetEnemy"));
        tagToAudioClip.Add("Player", Resources.Load<AudioClip>("Assets/Audio/SFX/RicochetPlayer"));
    }



    protected virtual void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Bullet collided with: {collision.gameObject.name}");

        
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
        if (tagToAudioClip.TryGetValue(collision.gameObject.tag, out AudioClip clip))
        {
            GameObject audioSourceObject = new GameObject("RicochetSound");
            audioSourceObject.transform.position = collision.contacts[0].point;

            AudioSource audioSource = audioSourceObject.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 1.0f; // 3D sound
            audioSource.minDistance = 1f;
            audioSource.maxDistance = 50f;

            audioSource.Play();

            // Destroy the audio source after 1 second
            Destroy(audioSourceObject, 1.0f);
        }
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

}