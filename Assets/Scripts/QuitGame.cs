using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void Quit()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SaveTotalGold();
        }

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

