using UnityEngine;
using System.Collections;

public class Gun : Weapon
{
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 1000.0f;
    public float bulletDamage = 10.0f;

    //buulet spread
    public float bulletSpread;

    // Fire modes
    public enum FireMode { Single, Burst, Automatic }
    public FireMode currentFireMode = FireMode.Single;

    // Burst mode
    public int burstCount = 3;
    private int currentBurst;
    public float burstDelay = 0.1f;

    // Automatic mode
    private bool isShooting;

    // Reference to the PlayerController
    public PlayerController playerController;
    public Transform playerCamera;

    // Store the original camera rotation
    private Vector3 originalCameraRotation;
    //Audio
    public AudioClip shootSound;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (playerCamera != null)
        {
            originalCameraRotation = playerCamera.localEulerAngles;
        }
    }

    private void Update()
    {
        // Cycle fire modes with the "F" key
        if (Input.GetKeyDown(KeyCode.F))
        {
            CycleFireMode();
        }

        // Handle shooting 
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            if (currentFireMode == FireMode.Single)
            {
                Shoot();
            }
            else if (currentFireMode == FireMode.Burst)
            {
                StartCoroutine(BurstFire());
            }
        }

        if (currentFireMode == FireMode.Automatic && Input.GetMouseButton(0) && !isShooting)
        {
            StartCoroutine(AutomaticFire());
        }

        //recoil through the PlayerController
        if (playerController != null)
        {
            playerController.ApplyRecoil(2.0f); // Example recoil amount
        }
    }

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

        // Calculate bullet direction with spread
        Vector3 direction = CalculateSpreadDirection();

        GameObject bulletObject = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.LookRotation(direction));
        Bullet bullet = bulletObject.GetComponent<Bullet>();

        if (bullet == null)
        {
            Debug.LogError("Bullet component is not found on the bullet prefab.");
            return;
        }

        bullet.speed = bulletSpeed;
        bullet.damage = bulletDamage;
        bullet.Initialize(direction);

        Debug.Log($"Bullet initialized with speed: {bulletSpeed}");

        if (shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
        else
        {
            Debug.LogWarning("Shoot sound is not assigned.");
        }
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
    private void CycleFireMode()
    {
        currentFireMode = (FireMode)(((int)currentFireMode + 1) % 3);
        Debug.Log($"Fire mode changed to: {currentFireMode}");
    }
    private IEnumerator BurstFire()
    {
        isShooting = true; //stop overlapping burst
        for (currentBurst = 0; currentBurst < burstCount; currentBurst++)
        {
            Shoot();
            yield return new WaitForSeconds(burstDelay);
        }
        isShooting = false;
    }

    private IEnumerator AutomaticFire()
    {
        isShooting = true;
        while (Input.GetMouseButton(0))
        {
            Shoot();
            yield return new WaitForSeconds(burstDelay); // Use burstDelay as the fire rate for automatic mode
        }
        isShooting = false;
    }
}
