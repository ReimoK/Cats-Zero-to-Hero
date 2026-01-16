using UnityEngine;
using UnityEngine.UI;

public class MainMenuSettingsUI : MonoBehaviour
{
    public GameObject menuRoot;      // MenuRoot panel
    public GameObject settingsRoot;  // SettingsRoot panel

    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        // Init sliders
        musicSlider.SetValueWithoutNotify(AudioManager.Instance.GetMusicVolume());
        sfxSlider.SetValueWithoutNotify(AudioManager.Instance.GetSfxVolume());

        // Hook slider events
        musicSlider.onValueChanged.AddListener(AudioManager.Instance.SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(AudioManager.Instance.SetSfxVolume);

        ShowMenu();
    }

    public void ShowSettings()
    {
        menuRoot.SetActive(false);
        settingsRoot.SetActive(true);
    }

    public void ShowMenu()
    {
        menuRoot.SetActive(true);
        settingsRoot.SetActive(false);
    }
}
