using UnityEngine;
using UnityEngine.SceneManagement;

public class WinContinue : MonoBehaviour
{
    // Set in Inspector:
    // street scene -> 1 (unlocks garden)
    // garden scene -> 2 (unlocks home)
    // home scene -> 2 (no next, or keep same)
    public int unlockThisIndex = 1;

    // Where to go after Continue (menu or next scene)
    public string sceneToLoad = "levels_menu";

    public void OnContinuePressed()
    {
        LevelProgress.UnlockIndex(unlockThisIndex);
        SceneManager.LoadScene(sceneToLoad);
    }
}
