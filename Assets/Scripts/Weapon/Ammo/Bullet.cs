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
    }



    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            Debug.Log("Bullet collided with " + collision.gameObject.name);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Bullet collided with " + collision.gameObject.name);
            HandleCollision(collision);
            Destroy(gameObject);
        }
    }

    protected void HandleCollision(Collision collision)
    {
        if (impactEffect != null)
        {
            Instantiate(impactEffect, collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal));
        }

        //sound played based off tag
        if (tagToAudioClip.TryGetValue(collision.gameObject.tag, out AudioClip clip))
        {
            GameObject audioSourceObject = new GameObject("RicochetSound");
            audioSourceObject.transform.position = collision.contacts[0].point;

            AudioSource audioSource = audioSourceObject.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 1.0f; // Make it 3D sound
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

            Debug.Log($"Bullet velocity: {rb.velocity.magnitude}");

            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator DespawnAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

}