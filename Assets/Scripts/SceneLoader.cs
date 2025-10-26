using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void RestartGame()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        Time.timeScale = 1f;
        SceneManager.LoadScene(currentSceneIndex);
    }
}