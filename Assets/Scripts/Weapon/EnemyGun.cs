using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : Weapon
{
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 500f;
    public float bulletDamage = 20f;

    public override void Shoot()
    {
        if (bulletPrefab == null || bulletSpawnPoint == null)
        {
            Debug.LogError("Bullet prefab or spawn point is not assigned.");
            return;
        }

        // Create a bullet and set its direction
        GameObject bulletObject = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Bullet bullet = bulletObject.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.speed = bulletSpeed;
            bullet.damage = bulletDamage;
            bullet.Initialize(bulletSpawnPoint.forward);
        }

        Debug.Log("Enemy shoots at the player!");
    }


}
