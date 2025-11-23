using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private float gameTime = 0f;

    [Header("UI References")]
    public ExperienceBar xpBar;
    public GameObject levelUpPanel;
    public GameObject winPanel;
    public GameObject losePanel;

    public TextMeshProUGUI button1Text;
    public TextMeshProUGUI button2Text;
    public TextMeshProUGUI button3Text;

    [Header("Stats")]
    public float currentXP = 0f;
    public int currentLevel = 1;
    public float xpToNextLevel = 10f;
    public int enemiesKilled = 0;
    public int enemiesToWin = 50;

    private bool isMilestoneLevel = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        Time.timeScale = 1f;
        if (levelUpPanel) levelUpPanel.SetActive(false);
        if (winPanel) winPanel.SetActive(false);
        if (losePanel) losePanel.SetActive(false);
    }

    void Start()
    {
        if (xpBar != null) xpBar.UpdateBar(currentXP, xpToNextLevel, currentLevel);
    }
    void Update()
    {
        if (Time.timeScale > 0)
        {
            gameTime += Time.deltaTime;

            if (HUDManager.Instance != null)
            {
                HUDManager.Instance.UpdateTimer(gameTime);
            }
        }
    }

    public void AddXP(float amount)
    {
        currentXP += amount;
        if (xpBar != null) xpBar.UpdateBar(currentXP, xpToNextLevel, currentLevel);

        if (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }
    }

    public void EnemyKilled()
    {
        enemiesKilled++;
        if (enemiesKilled >= enemiesToWin) WinGame();
    }

    private void LevelUp()
    {
        currentLevel++;
        currentXP -= xpToNextLevel;
        xpToNextLevel *= 1.2f;

        if (xpBar != null) xpBar.UpdateBar(currentXP, xpToNextLevel, currentLevel);

        if (currentLevel % 5 == 0)
        {
            isMilestoneLevel = true;
            GenerateAbilityOptions();
        }
        else
        {
            isMilestoneLevel = false;
            GenerateStatOptions();
        }

        if (levelUpPanel != null) levelUpPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    private void GenerateStatOptions()
    {
        if (button1Text) button1Text.text = "Max Health +1\n(Full Heal)";
        if (button2Text) button2Text.text = "Damage +10%";
        if (button3Text) button3Text.text = "Attack Speed +10%";
    }

    private void GenerateAbilityOptions()
    {
        if (button1Text) button1Text.text = "Empty";
        if (button2Text) button2Text.text = "Empty";
        if (button3Text) button3Text.text = "Empty";
    }

    public void ChooseUpgrade(int buttonIndex)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        WeaponController weapon = player.GetComponent<WeaponController>();
        PlayerHealth health = player.GetComponent<PlayerHealth>();

        if (isMilestoneLevel)
        {
            // ABILITIES
            switch (buttonIndex)
            {
                case 0: 
                    break;
                case 1:
                    break;
                case 2:
                    break;
            }
        }
        else
        {
            // APPLY STATS
            switch (buttonIndex)
            {
                case 0: // Health Upgrade
                    health.maxHealth += 1;
                    health.currentHealth = health.maxHealth;

                    if (HUDManager.Instance != null)
                    {
                        HUDManager.Instance.UpdateHealth(health.currentHealth, health.maxHealth);
                    }
                    break;
                case 1: // Damage
                    weapon.damage += 0.5f;
                    Debug.Log("Stat: Damage Up");
                    break;
                case 2: // Attack Speed 
                    weapon.fireRate *= 0.9f;
                    Debug.Log("Stat: Speed Up");
                    break;
            }
        }

        CloseLevelUpMenu();
    }

    private void CloseLevelUpMenu()
    {
        if (levelUpPanel != null) levelUpPanel.SetActive(false);
        Time.timeScale = 1f;

        if (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }
    }

    public void LoseGame()
    {
        if (winPanel.activeSelf || losePanel.activeSelf) return;

        Debug.Log("GAME OVER! You lost.");
        Time.timeScale = 0f;
        if (losePanel != null)
        {
            losePanel.SetActive(true);
        }
    }

    private void WinGame()
    {
        if (winPanel.activeSelf || losePanel.activeSelf) return;

        Debug.Log("YOU WIN! Goal Achieved.");
        Time.timeScale = 0f;
        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }
    }
    public void ForceWin()
    {
        WinGame();
    }
}