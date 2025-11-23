using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 3f;

    public float currentHealth;
    private GameManager gameManager;

    private float damageCooldown = 0.5f;
    private float nextDamageTime;

    void Start()
    {
        currentHealth = maxHealth;
        gameManager = GameManager.Instance;
        nextDamageTime = Time.time;

        if (HUDManager.Instance != null)
        {
            HUDManager.Instance.UpdateHealth(currentHealth, maxHealth);
        }
    }

    public void TakeDamage(float amount)
    {
        if (Time.time < nextDamageTime) return;

        currentHealth -= amount;
        nextDamageTime = Time.time + damageCooldown;

        if (HUDManager.Instance != null)
        {
            HUDManager.Instance.UpdateHealth(currentHealth, maxHealth);
        }

        if (currentHealth <= 0) Die();
    }
    public void Heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        if (HUDManager.Instance != null)
        {
            HUDManager.Instance.UpdateHealth(currentHealth, maxHealth);
        }
    }

    private void Die()
    {
        if (gameManager != null)
        {
            gameManager.LoseGame();
        }

        PlayerController pc = GetComponent<PlayerController>();
        if (pc != null) pc.enabled = false;

        WeaponController wc = GetComponent<WeaponController>();
        if (wc != null) wc.enabled = false;

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        EnemyHealth enemy = other.gameObject.GetComponent<EnemyHealth>();

        if (enemy != null)
        {
            TakeDamage(enemy.contactDamage);
        }
    }
}