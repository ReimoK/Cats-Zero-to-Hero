using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    [Header("UI")]
    public GameObject pauseMenuPanel;

    [Header("Popups that should block pausing (ESC disabled)")]
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject levelUpPanel;

    [Header("Audio Sliders (optional)")]
    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("Scene to load on Quit")]
    public string mainMenuSceneName = "main_title";

    private bool isOpen;

    void Start()
    {
        // Hook up sliders if assigned
        if (musicSlider != null && AudioManager.Instance != null)
        {
            musicSlider.SetValueWithoutNotify(AudioManager.Instance.GetMusicVolume());
            musicSlider.onValueChanged.AddListener(AudioManager.Instance.SetMusicVolume);
        }

        if (sfxSlider != null && AudioManager.Instance != null)
        {
            sfxSlider.SetValueWithoutNotify(AudioManager.Instance.GetSfxVolume());
            sfxSlider.onValueChanged.AddListener(AudioManager.Instance.SetSfxVolume);
        }

        SetOpen(false);
    }

    void Update()
    {
        // If any blocking popup is visible, prevent pause menu from opening
        if (IsBlockingPopupOpen())
        {
            // If pause menu is currently open, close it
            if (isOpen) SetOpen(false);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetOpen(!isOpen);
        }
    }

    private bool IsBlockingPopupOpen()
    {
        return (winPanel != null && winPanel.activeInHierarchy) ||
               (losePanel != null && losePanel.activeInHierarchy) ||
               (levelUpPanel != null && levelUpPanel.activeInHierarchy);
    }

    private void SetOpen(bool open)
    {
        isOpen = open;

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(open);

        // Pause/unpause game
        Time.timeScale = open ? 0f : 1f;
    }

    // Hook to Resume button
    public void OnResumePressed()
    {
        SetOpen(false);
    }

    // Hook to Quit button (to main menu)
    public void OnQuitPressed()
    {
        Time.timeScale = 1f; // important before leaving the level
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
