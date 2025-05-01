using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; // Required for scene management
using UnityEngine.Audio; // Required for audio management

public class WaveSpawner : MonoBehaviour
{
    public int totalWaves = 3;          // Total number of waves
    public int currentWave = 1;         // Current wave number
    public GameObject[] enemyPrefabs;     // The enemy prefab to spawn
    public Transform[] spawnPoints;    // An array of spawn points
    public int startingEnemies = 5;    // Number of enemies in the first wave
    public float timeBetweenWaves = 15f; // Time between waves in seconds
    public float timeBetweenSpawns = 1f; // Time between individual spawns in a wave
    public float spawnRadius = 5f;      // Radius around spawn points to spawn enemies
    public bool isSpawning = false;  // Is a wave currently spawning

    private List<GameObject> spawnedEnemies = new List<GameObject>(); // List of spawned enemies

    public TextMeshProUGUI waveText; // Text to display the current wave number
    //public GameManager GameManager; // Reference to the GameManager script

    [Header("Audio Clips")]
    public AudioClip waveMusic; // music to play when spawning enemies
    public AudioClip betweenWavesMusic; // music to play between waves

    void Start()
    {
        if (betweenWavesMusic != null)
        {
            AudioManager.Instance.PlayMusic(betweenWavesMusic); // Play the music between waves
        }
        if (waveMusic != null)
        {
            AudioManager.Instance.PlayMusic(waveMusic); // Play the music when spawning enemies
        }

        StartCoroutine(SpawnWave(startingEnemies));
    }

    IEnumerator SpawnWave(int enemyCount)
    {
        isSpawning = true;

       if (waveMusic != null)
        {
            AudioManager.Instance.PlayMusic(waveMusic); // Play the music when spawning enemies
        }

        waveText.text = "Wave " + currentWave; // Update the wave text
        Debug.Log($"Spawning wave {currentWave} with {enemyCount} enemies.");

        if (currentWave - 1 < 0 || currentWave - 1 >= enemyPrefabs.Length)
        {
            Debug.LogError($"Invalid enemy prefab index: {currentWave - 1}. Ensure enemyPrefabs array is properly configured.");
            yield break; // Exit the coroutine to prevent further errors
        }

        for (int i = 0; i < enemyCount; i++)
        {
            // Ensure the spawn point index is within bounds
            int spawnPointIndex = i % spawnPoints.Length;
            Vector3 spawnPosition = spawnPoints[spawnPointIndex].position + Random.insideUnitSphere * spawnRadius;
            spawnPosition.y = spawnPoints[spawnPointIndex].position.y; // Assuming you want to keep the enemies on the ground

            GameObject enemy = Instantiate(enemyPrefabs[currentWave - 1], spawnPosition, Quaternion.identity);

            EnemyStateController enemyController = enemy.GetComponent<EnemyStateController>();
            if (enemyController != null)
            {
                enemyController.Player = GameObject.FindWithTag("Player").transform; // Ensure the player is tagged as "Player"
            }

            spawnedEnemies.Add(enemy); // Add spawned enemy to the list
            yield return new WaitForSeconds(timeBetweenSpawns);
        }

        isSpawning = false;

        // Wait until all enemies are destroyed before starting the next wave
        yield return new WaitUntil(AreAllEnemiesDestroyed);

        if (betweenWavesMusic != null)
        {
            AudioManager.Instance.PlayMusic(betweenWavesMusic); // Play the music between waves
        }

        // Start the next wave if there are more waves to spawn
        if (currentWave < totalWaves)
        {
            currentWave++;
            yield return new WaitForSeconds(timeBetweenWaves);
            StartCoroutine(SpawnWave(startingEnemies + currentWave - 1)); // Increase enemy count for each wave
        }
        else
        {
            waveText.text = "All Waves Completed!"; // Update the wave text when all waves are completed
            Debug.Log("All waves completed! inWaitUntil spawnWaveEnum debug");
            // GameManager.Instance.gameOver = true; // Set game over in GameManager
            // GameManager.Instance.gameOverUI.ShowWinScreen(); // Show the win screen
            // Debug.Log("Game Over! You win! inWaitUntil spawnWaveEnum debug");
        }
    }

    // Coroutine to spawn enemies over time
    IEnumerator SpawnEnemies(int enemyCount, GameObject enemyPrefab)
    {
        for (int i = 0; i < enemyCount; i++)
        {
            // Ensure the spawn point index is within bounds
            int spawnPointIndex = i % spawnPoints.Length;
            Vector3 spawnPosition = spawnPoints[spawnPointIndex].position + Random.insideUnitSphere * spawnRadius;
            spawnPosition.y = 0; // Assuming you want to keep the enemies on the ground

            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            spawnedEnemies.Add(enemy); // Add spawned enemy to the list
            yield return new WaitForSeconds(timeBetweenSpawns);
        }

        isSpawning = false;
    }

    void SpawnEnemy()
    {
        // Pick a random spawn point
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[spawnIndex];

        // Randomize the spawn point within a radius
        Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
        spawnPoint.position += new Vector3(randomOffset.x, 0, randomOffset.y);

        // Spawn the enemy at the chosen spawn point
        Instantiate(enemyPrefabs[currentWave - 1], spawnPoint.position, spawnPoint.rotation);
    }

    public void RemoveEnemy(GameObject enemy)
    {
        GameObject rootObject = enemy.transform.root.gameObject; // Get the root object of the enemy
        if (spawnedEnemies.Contains(rootObject))
        {
            spawnedEnemies.Remove(rootObject);
            Debug.Log($"Enemy removed. Remaining enemies: {spawnedEnemies.Count}");
        }
        else
        {
            Debug.LogWarning("Attempted to remove an enemy that is not in the list.");
        }
    }

    // Check if all spawned enemies are destroyed
    public bool AreAllEnemiesDestroyed()
    {
        // Remove any null entries (destroyed enemies) from the list
        spawnedEnemies.RemoveAll(enemy => enemy == null);
        Debug.Log($"Enemies remaining: {spawnedEnemies.Count}");
        return spawnedEnemies.Count == 0;
    }


//     void Update()
//     {
//         if (currentWave > totalWaves && AreAllEnemiesDestroyed())
//         {
//         Debug.Log("All waves completed! update debug");
        
//         if (GameManager.Instance?.gameOver != null)
//         {
//             GameManager.Instance.gameOverUI.ShowWinScreen(); // Set game over in GameManager
//         }
//         else
//         {
//             Debug.LogError("GameManager instance is null or gameOver reference is missing.");
//         }
//     }
// }
}
