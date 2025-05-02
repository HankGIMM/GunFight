using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Mortar : Interactable
{
    public Camera mortarCamera; // Reference to the mortar camera
    public GameObject targetingCirclePrefab; // Prefab for the targeting circle
    public GameObject explosionEffectPrefab; // Prefab for the explosion effect (optional)
    public GameObject FireEffectPrefab; // Prefab for the fire effect (optional)
    public Transform fireEffectSpawnPoint; // Spawn point for the fire effect (optional)
    public AudioClip mortarFireSound; // Sound to play when firing the mortar (optional)
    public float targetingRadius = 50f; // Radius of the targeting circle
    public LayerMask groundLayer; // Layer mask for the ground



    private GameObject targetingCircleInstance;
    private AudioSource audioSource; // Reference to the AudioSource component

    public override void Interact(PlayerController playerController)
    {
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing from the Mortar GameObject.");
            return;
        }
        // Hide the pickup prompt
        PlayerHUD playerHUD = FindObjectOfType<PlayerHUD>();
        if (playerHUD != null)
        {
            playerHUD.HidePickupPrompt();
        }

        // Activate the mortar camera
        mortarCamera.gameObject.SetActive(true);

        // Enable the mortar camera's AudioListener and disable the player's AudioListener
        AudioListener playerAudioListener = playerController.playerCamera.GetComponent<AudioListener>();
        AudioListener mortarAudioListener = mortarCamera.GetComponent<AudioListener>();
        if (playerAudioListener != null) playerAudioListener.enabled = false;
        if (mortarAudioListener != null) mortarAudioListener.enabled = true;

        // Disable the player's camera
        playerController.playerCamera.gameObject.SetActive(false);
        playerController.enabled = false; // Disable player controls

        // Create the targeting circle
        targetingCircleInstance = Instantiate(targetingCirclePrefab);
    }

    private void Update()
    {
        if (mortarCamera.gameObject.activeSelf)
        {
            HandleTargeting();
        }
    }

    private void HandleTargeting()
    {
        // Ensure the targeting circle instance exists
        if (targetingCircleInstance == null)
        {
            Debug.LogWarning("Targeting circle instance is missing. Creating a new one.");
            targetingCircleInstance = Instantiate(targetingCirclePrefab);
        }
        // Raycast from the mortar camera to the ground
        Ray ray = mortarCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            // Position the targeting circle on the ground
            targetingCircleInstance.transform.position = hit.point;

            // Check for left mouse click
            if (Input.GetMouseButtonDown(0))
            {
                FireMortar();
                ExecuteMortarStrike(hit.point);
            }
        }
    }
    private void FireMortar()
    {
        // Play the firing sound
        if (mortarFireSound != null)
        {
            audioSource.PlayOneShot(mortarFireSound);
        }

        // Instantiate the muzzle flash effect
        if (FireEffectPrefab != null && fireEffectSpawnPoint != null)
        {
            GameObject fireEffect = Instantiate(FireEffectPrefab, fireEffectSpawnPoint.position, fireEffectSpawnPoint.rotation);
            Destroy(fireEffect, 1f); // Destroy the effect after 1 second
        }
    }

    private void ExecuteMortarStrike(Vector3 targetPosition)
    {
        // Find all enemies within the targeting radius
        Collider[] colliders = Physics.OverlapSphere(targetPosition, targetingRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                EnemyStateController enemyController = collider.GetComponent<EnemyStateController>();
                if (enemyController != null)
                {
                    // Transition the enemy to the DieState
                    Debug.Log($"Enemy {collider.name} is transitioning to DieState.");
                    enemyController.TransitionToState(new DieState(enemyController));
                }
                else
                {
                    Debug.LogWarning($"Enemy {collider.name} does not have an EnemyStateController.");
                }


                // Spawn a visual effect at the enemy's position
                if (explosionEffectPrefab != null)
                {
                    GameObject explosionEffect = Instantiate(explosionEffectPrefab, collider.transform.position, Quaternion.identity);
                    Destroy(explosionEffect, 6f); // Destroy after seconds
                }
                else
                {
                    Debug.LogWarning("Explosion effect prefab is not assigned.");
                }
            }
        }

        // Destroy the targeting circle
        Destroy(targetingCircleInstance);

        // Deactivate the mortar camera
        mortarCamera.gameObject.SetActive(false);

        // Reactivate the player's camera and controls
        PlayerController playerController = FindObjectOfType<PlayerController>();
        playerController.playerCamera.gameObject.SetActive(true);
        playerController.enabled = true; // Re-enable the PlayerController script

        // Enable the player's AudioListener and disable the mortar camera's AudioListener
        AudioListener playerAudioListener = playerController.playerCamera.GetComponent<AudioListener>();
        AudioListener mortarAudioListener = mortarCamera.GetComponent<AudioListener>();
        if (playerAudioListener != null) playerAudioListener.enabled = true;
        if (mortarAudioListener != null) mortarAudioListener.enabled = false;
    }
}
