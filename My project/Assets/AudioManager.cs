using UnityEngine;

/// <summary>
/// Simple AudioManager for main soundtrack and battle sounds
/// Integrates with existing MainMenu volume settings
/// </summary>
public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    
    [Header("Music")]
    public AudioClip mainSoundtrack;
    
    [Header("Battle Sound Effects")]
    public AudioClip attackSound;
    public AudioClip hitSound;
    public AudioClip superAttackSound;
    public AudioClip victorySound;
    public AudioClip defeatSound;
    
    public static AudioManager Instance { get; private set; }
    
    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("AudioManager: Instance created and set to persist across scenes");
        }
        else
        {
            Debug.Log("AudioManager: Duplicate instance found, destroying");
            Destroy(gameObject);
            return;
        }
        
        // Initialize audio sources if not assigned
        SetupAudioSources();
    }
    
    void Start()
    {
        // Start playing main soundtrack
        PlayMainSoundtrack();
        
        // Apply saved volume settings
        ApplyVolumeSettings();
    }
    
    void SetupAudioSources()
    {
        // Create music source if not assigned
        if (musicSource == null)
        {
            GameObject musicObj = new GameObject("MusicSource");
            musicObj.transform.SetParent(transform);
            musicSource = musicObj.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.playOnAwake = false;
            Debug.Log("AudioManager: Music source created");
        }
        
        // Create SFX source if not assigned
        if (sfxSource == null)
        {
            GameObject sfxObj = new GameObject("SFXSource");
            sfxObj.transform.SetParent(transform);
            sfxSource = sfxObj.AddComponent<AudioSource>();
            sfxSource.loop = false;
            sfxSource.playOnAwake = false;
            Debug.Log("AudioManager: SFX source created");
        }
    }
    
    void PlayMainSoundtrack()
    {
        if (mainSoundtrack != null && musicSource != null)
        {
            musicSource.clip = mainSoundtrack;
            musicSource.Play();
            Debug.Log("AudioManager: Main soundtrack started");
        }
        else
        {
            Debug.LogWarning("AudioManager: No main soundtrack assigned or music source missing!");
        }
    }
    
    void ApplyVolumeSettings()
    {
        // Get volume settings from MainMenuManager (using the existing system)
        float musicVolume = MainMenuManager.GetMusicVolume();
        float sfxVolume = MainMenuManager.GetSFXVolume();
        
        if (musicSource != null)
        {
            musicSource.volume = musicVolume;
            Debug.Log($"AudioManager: Music volume set to {musicVolume}");
        }
        
        if (sfxSource != null)
        {
            sfxSource.volume = sfxVolume;
            Debug.Log($"AudioManager: SFX volume set to {sfxVolume}");
        }
    }
    
    // Public methods for playing battle sounds
    public void PlayAttackSound()
    {
        PlaySFX(attackSound, "Attack");
    }
    
    public void PlayHitSound()
    {
        PlaySFX(hitSound, "Hit");
    }
    
    public void PlaySuperAttackSound()
    {
        PlaySFX(superAttackSound, "Super Attack");
    }
    
    public void PlayVictorySound()
    {
        PlaySFX(victorySound, "Victory");
    }
    
    public void PlayDefeatSound()
    {
        PlaySFX(defeatSound, "Defeat");
    }
    
    void PlaySFX(AudioClip clip, string soundName)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
            Debug.Log($"AudioManager: Playing {soundName} sound");
        }
        else
        {
            Debug.LogWarning($"AudioManager: {soundName} sound not assigned or SFX source missing!");
        }
    }
    
    // Public method to update volumes (called when settings change)
    public void UpdateVolumes()
    {
        ApplyVolumeSettings();
    }
    
    // Public method to stop music (if needed)
    public void StopMusic()
    {
        if (musicSource != null && musicSource.isPlaying)
        {
            musicSource.Stop();
            Debug.Log("AudioManager: Music stopped");
        }
    }
    
    // Public method to resume music (if needed)
    public void ResumeMusic()
    {
        if (musicSource != null && !musicSource.isPlaying && mainSoundtrack != null)
        {
            musicSource.Play();
            Debug.Log("AudioManager: Music resumed");
        }
    }
}
