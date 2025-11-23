using UnityEngine;
using UnityEngine.UI;
using TMPro;          

public class ExperienceBar : MonoBehaviour
{
    [Header("UI References")]
    public Image fillImage;
    public TextMeshProUGUI levelText;

    public void UpdateBar(float currentXP, float maxXP, int level)
    {
        float fillAmount = currentXP / maxXP;

        fillImage.fillAmount = fillAmount;

        if (levelText != null)
        {
            levelText.text = "Level " + level.ToString();
        }
    }
}