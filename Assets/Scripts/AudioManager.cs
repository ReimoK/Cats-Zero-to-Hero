using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public enum SoundType
    {
        Take_Damage,
        Powerup,
        Player_Shoot,
        Enemy_Shoot,
        Mousetrap,
        Button_Click,
        Music_Menu,
        Music_Battle
    }

    [System.Serializable]
    public class Sound
    {
        public SoundType Type;
        public AudioClip Clip;

        [Range(0f, 1f)]
        public float Volume = 1f;
    }

    public static AudioManager Instance;

    public Sound[] AllSounds;

    private Dictionary<SoundType, Sound> _soundDictionary = new Dictionary<SoundType, Sound>();

    private AudioSource _musicSource;

    // Master volume controls (0..1)
    [Range(0f, 1f)] public float masterSfx = 1f;
    [Range(0f, 1f)] public float masterMusic = 1f;

    const string SfxKey = "Vol_SFX";
    const string MusicKey = "Vol_Music";

    private void Awake()
    {
        // Singleton (prevents duplicates across scenes)
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Set up dictionary
        foreach (var s in AllSounds)
        {
            _soundDictionary[s.Type] = s;
        }

        // Create music source once
        if (_musicSource == null)
        {
            _musicSource = gameObject.AddComponent<AudioSource>();
            _musicSource.loop = true;
        }

        // Load saved volumes
        masterSfx = PlayerPrefs.GetFloat(SfxKey, 1f);
        masterMusic = PlayerPrefs.GetFloat(MusicKey, 1f);
        ApplyMusicVolume();
    }

    bool IsMusic(SoundType type)
    {
        return type == SoundType.Music_Menu || type == SoundType.Music_Battle;
    }

    // Play one-shot SFX
    public void Play(SoundType type)
    {
        if (!_soundDictionary.TryGetValue(type, out Sound s))
        {
            Debug.LogWarning($"Sound type {type} not found!");
            return;
        }

        // Don’t let music types be played as one-shots
        if (IsMusic(type))
        {
            Debug.LogWarning($"{type} is music. Use ChangeMusic() instead.");
            return;
        }

        var soundObj = new GameObject($"SFX_{type}");
        var audioSrc = soundObj.AddComponent<AudioSource>();

        audioSrc.clip = s.Clip;
        audioSrc.volume = s.Volume * masterSfx;
        audioSrc.Play();

        Destroy(soundObj, s.Clip.length);
    }

    // Swap looping music track
    public void ChangeMusic(SoundType type)
    {
        if (!_soundDictionary.TryGetValue(type, out Sound track))
        {
            Debug.LogWarning($"Music track {type} not found!");
            return;
        }

        if (_musicSource == null)
        {
            _musicSource = gameObject.AddComponent<AudioSource>();
            _musicSource.loop = true;
        }

        if (_musicSource.isPlaying && _musicSource.clip == track.Clip)
            return;

        _musicSource.clip = track.Clip;
        _musicSource.volume = track.Volume * masterMusic;
        _musicSource.Play();
    }

    void ApplyMusicVolume()
    {
        if (_musicSource != null && _musicSource.clip != null)
        {
            // Find the track volume from dictionary (optional but nice)
            // If you want simplest, just set _musicSource.volume = masterMusic;
            _musicSource.volume = masterMusic;
        }
    }

    // Called by sliders
    public void SetSfxVolume(float value01)
    {
        masterSfx = value01;
        PlayerPrefs.SetFloat(SfxKey, masterSfx);
        PlayerPrefs.Save();
    }

    public void SetMusicVolume(float value01)
    {
        masterMusic = value01;

        // Update currently playing music immediately
        // If you want per-track base volume, keep track of it; simplest is:
        if (_musicSource != null)
            _musicSource.volume = masterMusic;

        PlayerPrefs.SetFloat(MusicKey, masterMusic);
        PlayerPrefs.Save();
    }

    public float GetSfxVolume() => masterSfx;
    public float GetMusicVolume() => masterMusic;
}
