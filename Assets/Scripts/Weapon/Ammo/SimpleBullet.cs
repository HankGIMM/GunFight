using UnityEngine;

public class SimpleBullet : Bullet
{
    public override void Initialize(Vector3 direction)
    {
        rb.velocity = direction * speed;
    }
}