using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Settings")]
    public GameObject enemyPrefab;
    public float spawnRadius = 10f;
    public float timeBetweenWaves = 5f;

    [Header("Wave Config")]
    public float currentSpawnRate = 1f;
    public float spawnRateMultiplier = 0.1f;

    private float nextSpawnTime;
    private float nextWaveTime;
    private Transform player;
    private Vector2 screenBounds;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        if (spawnRadius < screenBounds.x)
        {
            spawnRadius = screenBounds.x + 2f;
        }
    }

    void Update()
    {
        if (player == null) return;

        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + (1f / currentSpawnRate);
        }

        if (Time.time >= nextWaveTime)
        {
            IncreaseDifficulty();
            nextWaveTime = Time.time + timeBetweenWaves;
        }
    }

    void SpawnEnemy()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector2 spawnPosition = (Vector2)player.position + (randomDirection * spawnRadius);

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    void IncreaseDifficulty()
    {
        currentSpawnRate += spawnRateMultiplier;
        Debug.Log($"Wave Complete! New Spawn Rate: {currentSpawnRate} enemies/sec");
    }
}