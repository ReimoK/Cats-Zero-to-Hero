using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void Quit()
    {
        // Asetab rakenduse kinni
        Application.Quit();

        // Debug log ainult editoris, kuna editoris ei sulgu rakendus tegelikult
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

