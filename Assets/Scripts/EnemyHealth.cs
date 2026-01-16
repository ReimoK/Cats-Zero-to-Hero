using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Stats")]
    public float maxHealth = 1f;
    public float contactDamage = 1f;

    [Header("Drops")]
    public GameObject xpGemPrefab;
    public GameObject moneyPrefab;
    public bool dropsMoneyInsteadOfXP = false;
    public bool isBoss = false;

    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }
    public void ApplyDifficultyBuff(float healthIncrease)
    {
        maxHealth += healthIncrease;

        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isBoss)
        {
            if (GameManager.Instance != null) GameManager.Instance.ForceWin();
            Destroy(gameObject);
            return;
        }

        GameObject toDrop = dropsMoneyInsteadOfXP ? moneyPrefab : xpGemPrefab;

        if (toDrop != null)
        {
            Instantiate(toDrop, transform.position, Quaternion.identity);
        }

        if (GameManager.Instance != null) GameManager.Instance.EnemyKilled();

        Destroy(gameObject);
    }
}