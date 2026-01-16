using UnityEngine;

public class SceneMusic : MonoBehaviour
{
    public AudioManager.SoundType musicType;

    void Start()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.ChangeMusic(musicType);
    }
}