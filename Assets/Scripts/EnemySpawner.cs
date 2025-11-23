using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Settings")]
    public GameObject enemyPrefab;
    public GameObject bossPrefab;
    public float spawnRadius = 10f;

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
        Instantiate(enemyPrefab, (Vector2)player.position + randomPos, Quaternion.identity);
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