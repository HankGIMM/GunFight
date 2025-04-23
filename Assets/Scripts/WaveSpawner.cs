using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    public int totalWaves = 3;          // Total number of waves
    public int currentWave = 1;         // Current wave number
    public GameObject[] enemyPrefabs;     // The enemy prefab to spawn
    public Transform[] spawnPoints;    // An array of spawn points
    public int startingEnemies = 5;    // Number of enemies in the first wave
    public float timeBetweenWaves = 10f; // Time between waves in seconds
    public float timeBetweenSpawns = 1f; // Time between individual spawns in a wave
    public float spawnRadius = 5f;      // Radius around spawn points to spawn enemies
    private bool isSpawning = false;  // Is a wave currently spawning

    private List<GameObject> spawnedEnemies = new List<GameObject>(); // List of spawned enemies

    public TextMeshProUGUI waveText; // Text to display the current wave number



    void Start()
    {
        StartCoroutine(SpawnWave(startingEnemies));
    }

    IEnumerator SpawnWave(int enemyCount)
    {
        isSpawning = true;

        waveText.text = "Wave " + currentWave; // Update the wave text

        if (currentWave == totalWaves)
        {
            enemyCount = 1; // Only one enemy should spawn during the final wave
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

        // Start the next wave if there are more waves to spawn
        if (currentWave < totalWaves)
        {
            currentWave++;
            yield return new WaitForSeconds(timeBetweenWaves);
            StartCoroutine(SpawnWave(startingEnemies + currentWave - 1)); // Increase enemy count for each wave
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

    // Check if all spawned enemies are destroyed
    public bool AreAllEnemiesDestroyed()
    {
        // Remove any null entries (destroyed enemies) from the list
        spawnedEnemies.RemoveAll(enemy => enemy == null);
        return spawnedEnemies.Count == 0;
    }
    // void Update()
    // {

    // }
}
