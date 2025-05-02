using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gun : Weapon
{
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 1000.0f;
    public float bulletDamage = 81.0f;

    // Ammo system
    public int currentAmmo = 30; // Current ammo in the magazine
    public int totalAmmo = 90; // Total ammo available
    public int magazineSize = 30; // Maximum ammo in a magazine
    public float reloadTime = 2f; // Time it takes to reload
    private bool isReloading = false;
    //public event Action<int, int> OnAmmoChanged; // Event to notify ammo changes

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

    public AudioClip reloadSound;

    //camera recoil
    public CameraRecoil cameraRecoil;

    //muzzle flash
    public ParticleSystem muzzleFlash;



    private void Start()
    {
        transform.localRotation = Quaternion.identity; // Reset local rotation to (0, 0, 0)

        //audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            GameObject assultTestObject = GameObject.Find("SFXAudioSource");
            if (assultTestObject != null)
            {
                audioSource = assultTestObject.GetComponent<AudioSource>();
                if (audioSource == null)
                {
                    Debug.LogError("AudioSource component not found on 'SFXAudioSource' after scene load.");
                }
            }
            else
            {
                Debug.LogError("GameObject 'SFXAudioSource' not found in the scene after scene load.");
            }
        }

        if (playerCamera != null)
        {
            originalCameraRotation = playerCamera.localEulerAngles;


        }
        if (muzzleFlash == null)
        {
            Debug.LogWarning("Muzzle flash particle system is not assigned.");
        }
    }

    private void Update()
    {
        if (isReloading)
            return; // Prevent shooting while reloading

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < magazineSize && totalAmmo > 0)
        {
            StartCoroutine(Reload());
        }

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
        if (PauseMenu.IsGamePaused) return; // Prevent shooting when the game is paused

        if (isReloading)
            return;

        if (currentAmmo <= 0)
        {
            Debug.Log("Out of ammo! Reload!");
            return;
        }

        currentAmmo--; // Decrease current ammo
        Debug.Log($"Shot fired! Ammo left: {currentAmmo}/{totalAmmo}");

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

        // Play muzzle flash particle system
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
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

    //reload
    private IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        if (reloadSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(reloadSound);
        }
        else
        {
            Debug.LogWarning("Reload sound is not assigned or AudioSource is missing.");
        }

        yield return new WaitForSeconds(reloadTime);

        int ammoToReload = Mathf.Min(magazineSize - currentAmmo, totalAmmo);
        currentAmmo += ammoToReload;
        totalAmmo -= ammoToReload;

        Debug.Log($"Reloaded! Ammo: {currentAmmo}/{totalAmmo}");
        isReloading = false;
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

    public void AddAmmo(int amount)
    {
        totalAmmo += amount;
        Debug.Log($"Added ammo: {amount}. Total ammo: {totalAmmo}");
    }
}
