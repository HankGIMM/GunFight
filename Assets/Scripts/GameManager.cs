using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management
using TMPro; // Required for TextMeshPro
using UnityEngine.UI; // Required for UI elements


public class GameManager : MonoBehaviour
{
    public WaveSpawner waveSpawner;
    public PlayerController playerController;

    public GameSceneManager sceneManager; // Reference to the SceneManager script

    private bool gameOver = false;

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

        if (waveSpawner.currentWave > waveSpawner.totalWaves && waveSpawner.AreAllEnemiesDestroyed())
        {
            Debug.Log("All waves complete! You win!");
            gameOver = true;

             // Transition to victory snapshot
            AudioManager.Instance.TransitionToSnapshot(AudioManager.Instance.victorySnapshot, 1.0f);

            sceneManager.ReturnToTitle(); // Return to the title screen
        }

        if (playerController.Health <= 0)
        {
            Debug.Log("Player is dead! Game Over!");
            gameOver = true;
            // sceneManager.RestartScene(); // Restart the current scene

            // Transition to game over snapshot
            AudioManager.Instance.TransitionToSnapshot(AudioManager.Instance.gameOverSnapshot, 1.0f);

            AudioManager.Instance.PlayMusic(AudioManager.Instance.gameOverMusic); // Play game over music
        }
    }
}
