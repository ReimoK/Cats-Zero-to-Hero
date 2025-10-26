using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector] public float damage = 1f;

    public GameObject hitEffectPrefab;

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);

            if (hitEffectPrefab != null)
            {
                Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }

    }
}