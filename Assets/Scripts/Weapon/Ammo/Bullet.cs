using UnityEditor;
using UnityEngine;
using System.Collections;

public abstract class Bullet : MonoBehaviour
{
    public float speed;
    public float damage;
    public Rigidbody rb;
    public GameObject impactEffect;

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
        if (collision.gameObject.CompareTag("Target"))
        {
            Debug.Log("Bullet collided with " + collision.gameObject.name);
            Destroy(gameObject);
        }
    }

    protected void HandleCollision(Collision collision)
    {
        if (impactEffect != null)
        {
            Instantiate(impactEffect, collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal));
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