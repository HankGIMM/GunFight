using UnityEngine;

public class Gun : Weapon
{
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 20.0f;
    public float bulletDamage = 10.0f;

    public override void Shoot()
    {

        if (bulletPrefab == null)
        {
            Debug.LogError("Bullet prefab is not assigned.");
            return;
        }

        if (bulletSpawnPoint == null)
        {
            Debug.LogError("Bullet spawn point is not assigned.");
            return;
        }

        GameObject bulletObject = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Bullet bullet = bulletObject.GetComponent<Bullet>();

        if (bullet == null)
        {
            Debug.LogError("Bullet component is not found on the bullet prefab.");
            return;
        }

        bullet.speed = bulletSpeed;
        bullet.damage = bulletDamage;
        bullet.Initialize(bulletSpawnPoint.forward);
    }
}