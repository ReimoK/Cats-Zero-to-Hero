using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Weapon Stats")]
    public GameObject projectilePrefab;
    public float damage = 1f;
    public float fireRate = 0.5f;
    public float projectileSpeed = 10f;
    public int projectileCount = 1;
    public float searchRadius = 15f;

    private PlayerController playerController;
    private float nextFireTime;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        nextFireTime = Time.time;
    }

    void Update()
    {
        if (playerController.isMoving) return;

        if (Time.time >= nextFireTime)
        {
            TryToFire();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void TryToFire()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, searchRadius, LayerMask.GetMask("Enemy"));
        Transform closestEnemy = null;
        float minDistance = Mathf.Infinity;

        foreach (Collider2D hit in hitColliders)
        {
            float distance = Vector2.Distance(transform.position, hit.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = hit.transform;
            }
        }

        if (closestEnemy != null)
        {
            Vector2 direction = (closestEnemy.position - transform.position).normalized;

            if (projectileCount == 1)
            {
                FireProjectile(direction, 0);
            }
            else
            {
                for (int i = 0; i < projectileCount; i++)
                {
                    float spread = -15f + (i * (30f / (projectileCount - 1)));
                    if (projectileCount == 1) spread = 0;

                    FireProjectile(direction, spread);
                }
            }
        }
    }

    private void FireProjectile(Vector2 direction, float angleOffset)
    {
        Vector2 finalDir = Quaternion.Euler(0, 0, angleOffset) * direction;

        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Rigidbody2D projRb = projectile.GetComponent<Rigidbody2D>();

        Projectile projScript = projectile.GetComponent<Projectile>();
        if (projScript != null) projScript.damage = damage;

        if (projRb != null)
        {
            projRb.linearVelocity = finalDir * projectileSpeed;
        }

        float angle = Mathf.Atan2(finalDir.y, finalDir.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.Euler(0, 0, angle);

        Destroy(projectile, 3f);
    }
}