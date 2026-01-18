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
        Music_Battle,
        Pickup_Coin,
        Pickup_XP
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
    private AudioSource _sfxSource;

    // Track currently playing music base volume so slider changes apply correctly
    private float _currentMusicBaseVolume = 1f;

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

        // Create sources once (persist across scenes)
        _musicSource = gameObject.AddComponent<AudioSource>();
        _musicSource.loop = true;

        _sfxSource = gameObject.AddComponent<AudioSource>();
        _sfxSource.loop = false;

        // Load saved volumes
        masterSfx = PlayerPrefs.GetFloat(SfxKey, 1f);
        masterMusic = PlayerPrefs.GetFloat(MusicKey, 1f);

        ApplyMusicVolume();
    }

    bool IsMusic(SoundType type)
    {
        return type == SoundType.Music_Menu || type == SoundType.Music_Battle;
    }

    // Play one-shot SFX (won't cut off when changing scenes)
    public void Play(SoundType type)
    {
        if (!_soundDictionary.TryGetValue(type, out Sound s))
        {
            Debug.LogWarning($"Sound type {type} not found!");
            return;
        }

        if (IsMusic(type))
        {
            Debug.LogWarning($"{type} is music. Use ChangeMusic() instead.");
            return;
        }

        if (s.Clip == null)
        {
            Debug.LogWarning($"Sound {type} has no clip assigned!");
            return;
        }

        // PlayOneShot allows overlapping SFX and continues through scene loads
        _sfxSource.PlayOneShot(s.Clip, s.Volume * masterSfx);
    }

    // Swap looping music track (won't restart if same clip already playing)
    public void ChangeMusic(SoundType type)
    {
        if (!_soundDictionary.TryGetValue(type, out Sound track))
        {
            Debug.LogWarning($"Music track {type} not found!");
            return;
        }

        if (track.Clip == null)
        {
            Debug.LogWarning($"Music track {type} has no clip assigned!");
            return;
        }

        if (_musicSource.isPlaying && _musicSource.clip == track.Clip)
            return; // keep playing seamlessly

        _musicSource.clip = track.Clip;
        _currentMusicBaseVolume = track.Volume;

        ApplyMusicVolume();
        _musicSource.Play();
    }

    void ApplyMusicVolume()
    {
        if (_musicSource != null)
        {
            // Apply both the track’s base volume and the slider master volume
            _musicSource.volume = _currentMusicBaseVolume * masterMusic;
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
        ApplyMusicVolume();

        PlayerPrefs.SetFloat(MusicKey, masterMusic);
        PlayerPrefs.Save();
    }

    public float GetSfxVolume() => masterSfx;
    public float GetMusicVolume() => masterMusic;
}
