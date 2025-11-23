using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector] public float damage = 1f;
    public GameObject hitEffectPrefab;

    [HideInInspector] public bool applyBind = false;
    [HideInInspector] public float bindDuration = 0f;
    [HideInInspector] public bool applySlow = false;
    [HideInInspector] public float slowAmount = 0f;
    [HideInInspector] public float slowDuration = 0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile") || other.CompareTag("Trap")) return;
        if (other.CompareTag("Player")) return;

        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
        EnemyAI enemyAI = other.GetComponent<EnemyAI>();

        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);

            if (enemyAI != null)
            {
                if (applyBind) enemyAI.ApplyBind(bindDuration);
                if (applySlow) enemyAI.ApplySlow(slowAmount, slowDuration);
            }

            if (hitEffectPrefab != null)
            {
                Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
}