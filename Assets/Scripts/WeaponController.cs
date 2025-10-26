using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float fireRate = 0.5f;
    public float projectileSpeed = 10f;
    public float searchRadius = 15f;

    private PlayerController playerController;
    private float nextFireTime;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("WeaponController requires a PlayerController on the same GameObject.");
        }
        nextFireTime = Time.time;
    }

    void Update()
    {
        if (playerController.isMoving)
        {
            return;
        }

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
            FireProjectile(direction);
        }
    }

    private void FireProjectile(Vector2 direction)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Rigidbody2D projRb = projectile.GetComponent<Rigidbody2D>();

        projectile.GetComponent<Projectile>().damage = 1f;

        if (projRb != null)
        {
            projRb.linearVelocity = direction * projectileSpeed;
        }

        Destroy(projectile, 3f);
    }
}