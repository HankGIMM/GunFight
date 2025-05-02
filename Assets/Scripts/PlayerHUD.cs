using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management
using TMPro; // Required for TextMeshPro
using UnityEngine.UI; // Required for UI elements

public class PlayerHUD : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Ammo UI")]
    public TextMeshProUGUI ammoText; // Text for ammo count

    [Header("Wave Info UI")]
    public TextMeshProUGUI waveText; // Text for current wave
    public TextMeshProUGUI waveCountdownText; // Text for wave countdown timer

    [Header("Player Health UI")]
    public TextMeshProUGUI healthText; // Text for player health

    [Header("Pickup promt UI")]
    public TextMeshProUGUI pickupPromptText; // Text for pickup prompt

    private PlayerController playerController;
    private WaveSpawner waveSpawner;

    private void Start()
    {
        // Find references to PlayerController and WaveSpawner
        playerController = FindObjectOfType<PlayerController>();
        waveSpawner = FindObjectOfType<WaveSpawner>();

        if (playerController == null)
        {
            Debug.LogError("PlayerController not found in the scene.");
        }

        if (waveSpawner == null)
        {
            Debug.LogError("WaveSpawner not found in the scene.");
        }
        // Hide the pickup prompt initially
        if (pickupPromptText != null)
        {
            pickupPromptText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (GameManageController.Instance != null && GameManageController.Instance.gameOver) return; // Stop updating if the game is over
        UpdateAmmoUI();
        UpdateWaveInfoUI();
        UpdateHealthUI();
        
    }

    public void UpdateAmmoUI()
    {
        if (playerController != null && playerController.equippedWeapon is Gun gun)
        {
            ammoText.text = $"Ammo: {gun.currentAmmo}/{gun.totalAmmo}";
        }
        else
        {
            ammoText.text = "Ammo: --/--";
        }
    }

    private void UpdateWaveInfoUI()
    {
        if (waveSpawner != null)
        {
            waveText.text = $"Wave: {waveSpawner.currentWave}/{waveSpawner.totalWaves}";

            if (waveSpawner.isSpawning)
            {
                waveCountdownText.text = "Wave in Progress";
            }
            else
            {
                waveCountdownText.text = $"Next Wave In: {Mathf.CeilToInt(waveSpawner.timeBetweenWaves)}s";
            }
        }
    }

    private void UpdateHealthUI()
    {
        if (playerController != null)
        {
            healthText.text = $"Health: {Mathf.CeilToInt(playerController.Health)}";
        }
        else
        {
            healthText.text = "Health: --";
        }
    }

    // Show the pickup prompt
    public void ShowPickupPrompt(string message)
    {
        if (pickupPromptText != null)
        {
            Debug.Log($"Showing pickup prompt: {message}");
            pickupPromptText.text = message;
            pickupPromptText.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("Pickup prompt text is not assigned in the Inspector.");
        }
    }

    // Hide the pickup prompt
    public void HidePickupPrompt()
    {
        if (pickupPromptText != null)
        {
            pickupPromptText.gameObject.SetActive(false);
        }
    }
}
