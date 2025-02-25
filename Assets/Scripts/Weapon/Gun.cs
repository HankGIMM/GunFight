using UnityEngine;

public class Gun : Weapon
{
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 20.0f;
    public float bulletDamage = 10.0f;

    public override void Shoot()
    {
        GameObject bulletObject = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Bullet bullet = bulletObject.GetComponent<Bullet>();
        bullet.speed = bulletSpeed;
        bullet.damage = bulletDamage;
        bullet.Initialize(bulletSpawnPoint.forward);
    }
}