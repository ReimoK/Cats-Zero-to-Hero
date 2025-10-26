using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject levelUpPanel;
    public GameObject winPanel;
    public GameObject losePanel;

    public float currentXP = 0f;
    public int currentLevel = 1;
    public float xpToNextLevel = 10f;

    public int enemiesKilled = 0;
    public int enemiesToWin = 5;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (levelUpPanel != null) levelUpPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);

        Time.timeScale = 1f;
    }

    public void AddXP(float amount)
    {
        currentXP += amount;
        if (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }
    }

    public void EnemyKilled()
    {
        enemiesKilled++;

        if (enemiesKilled >= enemiesToWin)
        {
            WinGame();
        }

        Debug.Log($"Enemies Killed: {enemiesKilled}/{enemiesToWin}");
    }

    private void LevelUp()
    {
        currentLevel++;
        currentXP -= xpToNextLevel;
        xpToNextLevel *= 1.5f;

        Time.timeScale = 0f;

        if (levelUpPanel != null)
        {
            levelUpPanel.SetActive(true);
        }

        if (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }
    }

    public void ChooseUpgrade(int upgradeIndex)
    {

        if (levelUpPanel != null)
        {
            levelUpPanel.SetActive(false);
        }

        Time.timeScale = 1f;
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
}