using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Economy")]
    public int totalGold = 0;

    [Header("UI References")]
    public ExperienceBar xpBar;
    public GameObject levelUpPanel;
    public GameObject winPanel;
    public GameObject losePanel;

    [Header("Upgrade Buttons (Text)")]
    public TextMeshProUGUI button1Text;
    public TextMeshProUGUI button2Text;
    public TextMeshProUGUI button3Text;

    [Header("Upgrade Buttons (Icons)")]
    public Image button1Icon;
    public Image button2Icon;
    public Image button3Icon;

    [Header("Stat Sprites")]
    public Sprite healthIcon;
    public Sprite damageIcon;
    public Sprite speedIcon;

    [Header("Ability Sprites")]
    public Sprite yarnIcon;
    public Sprite milkIcon;
    public Sprite trapIcon;
    public Sprite cheeseIcon;

    [Header("Game Stats")]
    public float currentXP = 0f;
    public int currentLevel = 1;
    public float xpToNextLevel = 10f;
    public int enemiesKilled = 0;
    public int enemiesToWin = 50;

    private bool isMilestoneLevel = false;
    private float gameTime = 0f;

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
        if (xpBar != null)
            xpBar.UpdateBar(currentXP, xpToNextLevel, currentLevel);
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
    public void AddGold(int amount)
    {
        totalGold += amount;
        HUDManager.Instance.UpdateGold(totalGold);
    }

    public void AddXP(float amount)
    {
        currentXP += amount;

        if (xpBar != null)
            xpBar.UpdateBar(currentXP, xpToNextLevel, currentLevel);

        if (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }
    }

    public void EnemyKilled()
    {
        enemiesKilled++;
    }

    private void LevelUp()
    {
        AudioManager.Instance.Play(AudioManager.SoundType.Powerup);
        currentLevel++;
        currentXP -= xpToNextLevel;
        xpToNextLevel *= 1.2f;

        if (xpBar != null)
            xpBar.UpdateBar(currentXP, xpToNextLevel, currentLevel);

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
        if (button1Text) button1Text.text = "+1 Max HP & Full Heal";
        if (button2Text) button2Text.text = "Damage +1";
        if (button3Text) button3Text.text = "+10% Fire Rate";

        if (button1Icon) button1Icon.sprite = healthIcon;
        if (button2Icon) button2Icon.sprite = damageIcon;
        if (button3Icon) button3Icon.sprite = speedIcon;
    }

    private void GenerateAbilityOptions()
    {
        AbilityManager abilities = GameObject.FindGameObjectWithTag("Player").GetComponent<AbilityManager>();

        if (button1Text) button1Text.text = "Yarn Ball";
        if (button1Icon) button1Icon.sprite = yarnIcon;

        if (button2Text) button2Text.text = "Spilled Milk";
        if (button2Icon) button2Icon.sprite = milkIcon;

        if (abilities != null && abilities.hasTraps)
        {
            if (button3Text) button3Text.text = "Stinky Cheese";
            if (button3Icon) button3Icon.sprite = cheeseIcon;
        }
        else
        {
            if (button3Text) button3Text.text = "Mouse Trap";
            if (button3Icon) button3Icon.sprite = trapIcon;
        }
    }


    public void ChooseUpgrade(int buttonIndex)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        WeaponController weapon = player.GetComponent<WeaponController>();
        PlayerHealth health = player.GetComponent<PlayerHealth>();
        AbilityManager abilities = player.GetComponent<AbilityManager>();

        if (isMilestoneLevel)
        {
            switch (buttonIndex)
            {
                case 0: // Yarn
                    if (abilities) abilities.hasYarn = true;
                    Debug.Log("Selected: Yarn Ball");
                    break;

                case 1: // Milk
                    if (abilities) abilities.hasMilk = true;
                    Debug.Log("Selected: Spilled Milk");
                    break;

                case 2: // Traps / Cheese
                    if (abilities)
                    {
                        if (!abilities.hasTraps)
                        {
                            abilities.hasTraps = true;
                            Debug.Log("Selected: Mouse Traps");
                        }
                        else
                        {
                            abilities.hasCheeseUpgrade = true;
                            Debug.Log("Selected: Cheese Upgrade");
                        }
                    }
                    break;
            }
        }
        else
        {
            switch (buttonIndex)
            {
                case 0: // Health
                    health.maxHealth += 1;
                    health.Heal(health.maxHealth);
                    Debug.Log("Selected: Health Up");
                    break;

                case 1: // Damage
                    weapon.damage += 1f;
                    Debug.Log("Selected: Damage Up");
                    break;

                case 2: // Attack Speed
                    weapon.fireRate *= 0.9f;
                    Debug.Log("Selected: Speed Up");
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

    public void ForceWin()
    {
        WinGame();
    }

    public void SaveTotalGold()
    {
        int savedGold = PlayerPrefs.GetInt("SavedGold", 0);

        int newTotal = savedGold + totalGold;

        PlayerPrefs.SetInt("SavedGold", newTotal);
        PlayerPrefs.Save();

        Debug.Log("Gold Saved! New Total: " + newTotal);
    }

    public void WinGame()
    {
        SaveTotalGold();
        Time.timeScale = 0f;
        if (winPanel != null) winPanel.SetActive(true);
    }

    public void LoseGame()
    {
        if (winPanel.activeSelf || losePanel.activeSelf) return;

        SaveTotalGold();

        Debug.Log("GAME OVER!");
        Time.timeScale = 0f;
        if (losePanel != null) losePanel.SetActive(true);
    }
}