using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    public float speed;
    public float damage;
    public Rigidbody rb;

    public abstract void Initialize(Vector3 direction);

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        // Handle collision logic here
        Destroy(gameObject);
    }
}