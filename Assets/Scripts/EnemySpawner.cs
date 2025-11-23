using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Settings")]
    public GameObject enemyPrefab;
    public GameObject bossPrefab;
    public float spawnRadius = 10f;

    [Header("Difficulty Scaling")]
    public float healthGainPerMinute = 2f;  // +2 HP every minute
    public float speedGainPerMinute = 0.1f; // +0.1 Speed every minute

    [Header("Timing")]
    public float bossSpawnTime = 60f; 
    private bool bossSpawned = false;

    private float nextSpawnTime;
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null || bossSpawned) return;

        if (Time.timeSinceLevelLoad >= bossSpawnTime)
        {
            SpawnBoss();
            return;
        }

        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + 1f;
        }
    }

    void SpawnEnemy()
    {
        Vector2 randomPos = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPos = (Vector2)player.position + randomPos;

        GameObject newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

        float timeInMinutes = Time.timeSinceLevelLoad / 60f;

        float extraHealth = timeInMinutes * healthGainPerMinute;
        float extraSpeed = timeInMinutes * speedGainPerMinute;

        EnemyHealth healthScript = newEnemy.GetComponent<EnemyHealth>();
        if (healthScript != null)
        {
            healthScript.ApplyDifficultyBuff(extraHealth);
        }

        EnemyAI movementScript = newEnemy.GetComponent<EnemyAI>();
        if (movementScript != null)
        {
            movementScript.currentMoveSpeed += extraSpeed;
        }
    }

    void SpawnBoss()
    {
        bossSpawned = true;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }

        Instantiate(bossPrefab, Vector3.zero, Quaternion.identity);

        Debug.Log("WARNING: BOSS SPAWNED!");
    }
}