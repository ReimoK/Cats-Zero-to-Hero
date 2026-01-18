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

    [Header("Skins System")]
    public Sprite[] bulletSkins;

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
            Renderer r = hit.GetComponentInChildren<Renderer>();
            if (r == null || !r.isVisible)
                continue;

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

            int skinID = PlayerPrefs.GetInt("EquippedSkin", 0);

            for (int i = 0; i < projectileCount; i++)
            {
                float spread = (projectileCount == 1) ? 0 : -15f + (i * (30f / (projectileCount - 1)));
                FireProjectile(direction, spread, skinID);
            }
        }
    }

    private void FireProjectile(Vector2 direction, float angleOffset, int skinID)
    {
        Vector2 finalDir = Quaternion.Euler(0, 0, angleOffset) * direction;

        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Rigidbody2D projRb = projectile.GetComponent<Rigidbody2D>();
        Projectile projScript = projectile.GetComponent<Projectile>();

        if (projScript != null)
        {
            projScript.damage = damage;

            if (skinID < bulletSkins.Length && bulletSkins[skinID] != null)
            {
                projScript.SetBulletSprite(bulletSkins[skinID]);
            }
        }

        if (projRb != null)
        {
            projRb.linearVelocity = finalDir * projectileSpeed;
        }

        float angle = Mathf.Atan2(finalDir.y, finalDir.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}