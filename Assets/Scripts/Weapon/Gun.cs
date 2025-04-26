using UnityEngine;
using System.Collections;

public class Gun : Weapon
{
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 1000.0f;
    public float bulletDamage = 81.0f;

    //bullet spread
    public float bulletSpread;
    public float defaultBulletSpread = 0.1f;
    public float adsBulletSpread = 0.02f;

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

    //camera recoil
    public CameraRecoil cameraRecoil;

    private void Start()
    {
        transform.localRotation = Quaternion.identity; // Reset local rotation to (0, 0, 0)

        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing from the Gun GameObject.");
        }

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
            if (currentFireMode == FireMode.Single && !isShooting)
            {
                Shoot();
                
            }
            else if (currentFireMode == FireMode.Burst && !isShooting)
            {
                StartCoroutine(BurstFire());
            }
        }

        if (currentFireMode == FireMode.Automatic && Input.GetMouseButton(0) && !isShooting)
        {
            StartCoroutine(AutomaticFire());
        }

        if (Input.GetMouseButtonUp(0))
        {
            isShooting = false; // Reset shooting state when the button is released
        }

        // Handle ADS (Aim Down Sights)
        if (Input.GetMouseButton(1)) // Right mouse button
        {
            isAiming = true;
            bulletSpread = adsBulletSpread;
            AimDownSights();
        }
        else
        {
            isAiming = false;
            bulletSpread = defaultBulletSpread;
            ReturnToDefaultPosition();
        }

    }

    public override void Shoot()
    {
        // if (isShooting) return; // Prevent overlapping shooting
        // isShooting = true;


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

        //  Debug.Log($"Bullet initialized with speed: {bulletSpeed}");

        if (shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
        else
        {
            Debug.LogWarning("Shoot sound is not assigned.");
        }

        // isShooting = false; // Reset shooting state after shoot
        ApplyRecoil();
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
            ApplyRecoil();
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
            ApplyRecoil();
            yield return new WaitForSeconds(burstDelay); // Use burstDelay as the fire rate for automatic mode
        }

    }

    private void ApplyRecoil()
    {
        if (cameraRecoil != null)
        {
            cameraRecoil.AddRecoil(4.0f); // Example recoil amount
        }
        else
        {
            Debug.LogWarning("CameraRecoil script is not assigned to the Gun.");
        }
    }
}
