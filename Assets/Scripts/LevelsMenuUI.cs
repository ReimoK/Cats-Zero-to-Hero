using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelsMenuUI : MonoBehaviour
{
    [Header("Assign buttons")]
    public Button streetButton;
    public Button gardenButton;
    public Button homeButton;

    [Header("Optional: lock overlays")]
    public GameObject gardenLockIcon;
    public GameObject homeLockIcon;

    void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        // Street is always unlocked (index 0)
        SetButtonLocked(streetButton, false, null);

        // Garden requires unlock index >= 1
        SetButtonLocked(gardenButton, !LevelProgress.IsUnlocked(1), gardenLockIcon);

        // Home requires unlock index >= 2
        SetButtonLocked(homeButton, !LevelProgress.IsUnlocked(2), homeLockIcon);
    }

    void SetButtonLocked(Button button, bool locked, GameObject lockIcon)
    {
        if (button != null) button.interactable = !locked;
        if (lockIcon != null) lockIcon.SetActive(locked);
    }

    // Hook these to button OnClick
    public void PlayStreet() => SceneManager.LoadScene("level_street");
    public void PlayGarden() => SceneManager.LoadScene("level_garden");
    public void PlayHome() => SceneManager.LoadScene("level_home");
}
