using UnityEngine;

public class BossAI : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;
    private Transform player;
    private Rigidbody2D rb;

    [Header("Combat")]
    public GameObject enemyProjectilePrefab;
    public float fireRate = 2f;
    public int projectilesPerBurst = 12;
    public float projectileSpeed = 4f;

    private float nextFireTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    void FixedUpdate()
    {
        if (player == null) return;

        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;

        if (Time.time >= nextFireTime)
        {
            FireRadialBurst();
            AudioManager.Instance.Play(AudioManager.SoundType.Enemy_Shoot);
            nextFireTime = Time.time + fireRate;
        }
    }

    void FireRadialBurst()
    {
        float angleStep = 360f / projectilesPerBurst;
        float angle = 0f;

        for (int i = 0; i < projectilesPerBurst; i++)
        {
            float projectileDirXPosition = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
            float projectileDirYPosition = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

            Vector2 projectileVector = new Vector2(projectileDirXPosition, projectileDirYPosition);
            Vector2 projectileMoveDirection = (projectileVector - (Vector2)transform.position).normalized;

            GameObject tmpObj = Instantiate(enemyProjectilePrefab, transform.position, Quaternion.identity);

            Rigidbody2D projRB = tmpObj.GetComponent<Rigidbody2D>();
            if (projRB != null)
            {
                projRB.linearVelocity = projectileMoveDirection * projectileSpeed;
            }

            angle += angleStep;
        }
    }
}