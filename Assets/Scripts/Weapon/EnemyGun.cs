using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class EnemyGun : Weapon
{

    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 800f;
    public float bulletDamage = 20f;

    public float bulletSpread = 0.3f; // Spread for the enemy gun

    public ParticleSystem muzzleFlash; // Reference to the muzzle flash particle system

    public AudioSource audioSource; // Reference to the AudioSource component
    public AudioClip shootSound; // Sound to play when shooting



    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found on the enemy gun.");

        }

        if (muzzleFlash == null)
        {
            Debug.LogError("Muzzle flash particle system not assigned.");
        }
    }
    public override void Shoot()
    {
        if (bulletPrefab == null || bulletSpawnPoint == null)
        {
            Debug.LogError("Bullet prefab or spawn point is not assigned.");
            return;
        }

        // Play the muzzle flash effect
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        // Play the shooting sound

        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
        else
        {
            Debug.LogError("AudioSource or shoot sound not assigned.");
        }

        Vector3 direction = CalculateSpreadDirection();

        // Create a bullet and set its direction
        GameObject bulletObject = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.LookRotation(direction));
        Bullet bullet = bulletObject.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.speed = bulletSpeed;
            bullet.damage = bulletDamage;
            //bullet.isEnemyBullet = true; // Set the bullet as an enemy bullet
            bullet.Initialize(bulletSpawnPoint.forward);
            Debug.Log($"Bullet instantiated with damage: {bullet.damage}");
        }

        Debug.Log("Enemy shoots at the player!");
    }

    private Vector3 CalculateSpreadDirection()
    {
        // Get the forward direction of the gun
        Vector3 forward = bulletSpawnPoint.forward;

        // Apply random spread
        float randomX = Random.Range(-bulletSpread, bulletSpread);
        float randomY = Random.Range(-bulletSpread, bulletSpread);

        // Rotate the forward vector by the random spread angles
        Quaternion spreadRotation = Quaternion.Euler(randomX, randomY, 0);
        return spreadRotation * forward;
    }
}
