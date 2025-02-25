using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
     public float speed;
    public float damage;
    public Rigidbody rb;
    public GameObject impactEffect;

    public abstract void Initialize(Vector3 direction);

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        HandleCollision(collision);
        Destroy(gameObject);
    }

    protected void HandleCollision(Collision collision)
    {
        if (impactEffect != null)
        {
            Instantiate(impactEffect, collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal));
        }

        // Enemy enemy = collision.collider.GetComponent<Enemy>();
        // if (enemy != null)
        // {
        //     enemy.TakeDamage(damage);
        // }
    }
}