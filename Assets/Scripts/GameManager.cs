using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public WaveSpawner waveSpawner;
    public PlayerController playerController;

    public SceneManager sceneManager; // Reference to the SceneManager script

    private bool gameOver = false;

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
            sceneManager.ReturnToTitle(); // Return to the title screen
        }

        if (playerController.Health <= 0)
        {
            Debug.Log("Player is dead! Game Over!");
            gameOver = true;
            sceneManager.RestartScene(); // Restart the current scene
        }
    }
}
