using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public enum SpawnPhase { Wave1, MiniBoss, Wave2, FinalBoss, LevelComplete }

    [Header("Level Configuration")]
    public int levelNumber = 1;
    public SpawnPhase currentPhase = SpawnPhase.Wave1;

    [Header("Enemy Prefabs")]
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    public GameObject moneyEnemy;
    public GameObject miniBossPrefab;
    public GameObject finalBossPrefab;

    [Header("Settings")]
    public float spawnRadius = 15f;
    public float spawnRate = 1.0f;
    [Range(0, 100)] public float moneyEnemyChance = 5f;

    private float phaseTimer = 0f;
    private float nextSpawnTime;
    private Transform player;
    private GameObject activeBoss;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        phaseTimer = 5 * 60f;
    }

    void Update()
    {
        if (player == null || currentPhase == SpawnPhase.LevelComplete) return;

        HandlePhases();

        if ((currentPhase == SpawnPhase.Wave1 || currentPhase == SpawnPhase.Wave2) && Time.time >= nextSpawnTime)
        {
            SpawnRegularEnemy();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    void HandlePhases()
    {
        switch (currentPhase)
        {
            case SpawnPhase.Wave1:
                phaseTimer -= Time.deltaTime;
                if (phaseTimer <= 0)
                {
                    SpawnBoss(miniBossPrefab);
                    currentPhase = SpawnPhase.MiniBoss;
                }
                break;

            case SpawnPhase.MiniBoss:
                
                if (activeBoss == null)
                {
                    phaseTimer = 5 * 60f;
                    currentPhase = SpawnPhase.Wave2;
                }
                break;

            case SpawnPhase.Wave2:
                phaseTimer -= Time.deltaTime;
                if (phaseTimer <= 0)
                {
                    SpawnBoss(finalBossPrefab);
                    currentPhase = SpawnPhase.FinalBoss;
                }
                break;

            case SpawnPhase.FinalBoss:
                if (activeBoss == null)
                {
                    currentPhase = SpawnPhase.LevelComplete;
                    GameManager.Instance.WinGame();
                }
                break;
        }
    }

    void SpawnRegularEnemy()
    {
        // 1. Roll for Money
        if (Random.Range(0f, 100f) <= moneyEnemyChance)
        {
            InstantiateEnemy(moneyEnemy);
            return;
        }

        // 2. Wave 1
        if (currentPhase == SpawnPhase.Wave1)
        {
            if (levelNumber == 1 || levelNumber == 2) InstantiateEnemy(enemy1);
            if (levelNumber == 3) InstantiateEnemy(Random.value > 0.5f ? enemy1 : enemy2);
        }
        // 3. Wave 2
        else if (currentPhase == SpawnPhase.Wave2)
        {
            if (levelNumber == 1) InstantiateEnemy(enemy1);
            if (levelNumber == 2) InstantiateEnemy(Random.value > 0.5f ? enemy1 : enemy2);
            if (levelNumber == 3)
            {
                float rand = Random.value;
                if (rand < 0.33f) InstantiateEnemy(enemy1);
                else if (rand < 0.66f) InstantiateEnemy(enemy2);
                else InstantiateEnemy(enemy3);
            }
        }
    }

    void InstantiateEnemy(GameObject prefab)
    {
        if (prefab == null) return;
        Vector2 spawnPos = (Vector2)player.position + Random.insideUnitCircle.normalized * spawnRadius;
        Instantiate(prefab, spawnPos, Quaternion.identity);
    }

    void SpawnBoss(GameObject bossPrefab)
    {
        Vector2 spawnPos = (Vector2)player.position + Random.insideUnitCircle.normalized * spawnRadius;
        activeBoss = Instantiate(bossPrefab, spawnPos, Quaternion.identity);

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (enemy != activeBoss) Destroy(enemy);
        }
    }
}