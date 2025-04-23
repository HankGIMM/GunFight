using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public WaveSpawner waveSpawner;
    public PlayerController playerController;

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
        }

        if (playerController.Health <= 0)
        {
            Debug.Log("Player is dead! Game Over!");
            gameOver = true;
        }
    }
}
