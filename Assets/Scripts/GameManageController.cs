using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management
using TMPro; // Required for TextMeshPro
using UnityEngine.UI; // Required for UI elements



public class GameManageController : MonoBehaviour
{
    public static GameManageController Instance; // Singleton instance
    public WaveSpawner waveSpawner;
    public PlayerController playerController;
    public GameOverUI gameOverUI; // Reference to the GameOverUI script
    public PlayerHUD playerHUD; // Reference to the PlayerHUD script

    public GameSceneManager sceneManager; // Reference to the SceneManager script

    public bool gameOver = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // Set the singleton instance
           // DontDestroyOnLoad(gameObject); // Keep this object across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }

        // DontDestroyOnLoad(gameObject); // Keep this object across scenes
    }
    private void Start()
    {
        AudioManager.Instance.PlayMusic(AudioManager.Instance.gameplayMusic); // Play gameplay music
    }
    private void Update()
    {
        if (gameOver)
        {
            Debug.Log("Game Over!");
            return;
        }

        // Check for lose condition
        if (playerController.Health <= 0)
        {
            Debug.Log("Player is dead! Game Over!");
            gameOver = true;

            if (playerHUD != null)
            {
                playerHUD.gameObject.SetActive(false); // Hide the PlayerHUD
            }
            else
            {
                Debug.LogError("PlayerHUD is not assigned in the Inspector.");
            }

            if (gameOverUI != null)
            {
                gameOverUI.ShowLoseScreen();
            }
            else
            {
                Debug.LogError("GameOverUI is not assigned in the Inspector.");
            }
        }
    }

    public void EndGame()
    {
        gameOver = true; // Set game over state
        Time.timeScale = 0f; // Pause the game
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true; // Show the cursor

        Debug.Log("All waves complete! You win!");

        if (playerHUD != null)
        {
            playerHUD.gameObject.SetActive(false); // Hide the PlayerHUD
        }
        else
        {
            Debug.LogError("PlayerHUD is not assigned in the Inspector.");
        }

        // Show the victory screen
        if (gameOverUI != null)
        {
            gameOverUI.ShowWinScreen();
        }
        else
        {
            Debug.LogError("GameOverUI is not assigned in the Inspector.");
        }
    }
}
