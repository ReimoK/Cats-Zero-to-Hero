using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;

    [Header("UI References")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI healthText;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UpdateTimer(float timeInSeconds)
    {
        if (timerText == null) return;

        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        if (healthText == null) return;

        healthText.text = $"HP: {currentHealth} / {maxHealth}";
    }
}