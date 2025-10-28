using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [Header("Menu Buttons")]
    public Button newGameButton;
    public Button loadGameButton;
    public Button settingsButton;
    public Button quitButton;
    
    [Header("Menu Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    
    [Header("Settings UI")]
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public Button backToMenuButton;
    
    [Header("Audio")]
    public AudioSource backgroundMusicSource;
    
    [Header("Scene Names")]
    public string mapSceneName = "MapScene";
    public string characterSelectionSceneName = "BattleScene"; // We'll use battle scene for character selection
    
    private void Start()
    {
        Debug.Log("MainMenuManager: Starting main menu...");
        
        // Setup button listeners
        SetupButtonListeners();
        
        // Setup settings listeners
        SetupSettingsListeners();
        
        // Initialize audio settings
        InitializeAudioSettings();
        
        // Show main menu panel
        ShowMainMenu();
        
        // Play background music
        PlayBackgroundMusic();
        
        Debug.Log("MainMenuManager: Main menu initialized successfully!");
    }
    
    private void SetupButtonListeners()
    {
        if (newGameButton != null)
        {
            newGameButton.onClick.AddListener(OnNewGameClicked);
            Debug.Log("MainMenuManager: New Game button listener added");
        }
        
        if (loadGameButton != null)
        {
            loadGameButton.onClick.AddListener(OnLoadGameClicked);
            Debug.Log("MainMenuManager: Load Game button listener added");
        }
        
        if (settingsButton != null)
        {
            settingsButton.onClick.AddListener(OnSettingsClicked);
            Debug.Log("MainMenuManager: Settings button listener added");
        }
        
        if (quitButton != null)
        {
            quitButton.onClick.AddListener(OnQuitClicked);
            Debug.Log("MainMenuManager: Quit button listener added");
        }
    }
    
    private void SetupSettingsListeners()
    {
        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        }
        
        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        }
        
        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        }
        
        if (backToMenuButton != null)
        {
            backToMenuButton.onClick.AddListener(OnBackToMenuClicked);
        }
    }
    
    private void InitializeAudioSettings()
    {
        // Load saved audio settings or use defaults
        float masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.8f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
        
        if (masterVolumeSlider != null) masterVolumeSlider.value = masterVolume;
        if (musicVolumeSlider != null) musicVolumeSlider.value = musicVolume;
        if (sfxVolumeSlider != null) sfxVolumeSlider.value = sfxVolume;
        
        // Apply volume settings
        AudioListener.volume = masterVolume;
        if (backgroundMusicSource != null)
        {
            backgroundMusicSource.volume = musicVolume;
        }
        
        Debug.Log($"MainMenuManager: Audio initialized - Master: {masterVolume}, Music: {musicVolume}, SFX: {sfxVolume}");
    }
    
    private void PlayBackgroundMusic()
    {
        if (backgroundMusicSource != null && backgroundMusicSource.clip != null)
        {
            backgroundMusicSource.Play();
            Debug.Log("MainMenuManager: Background music started");
        }
        else
        {
            Debug.LogWarning("MainMenuManager: No background music source or clip assigned!");
        }
    }
    
    public void OnNewGameClicked()
    {
        Debug.Log("MainMenuManager: New Game clicked - Starting fresh game...");
        
        // Delete the actual save file to ensure fresh start
        SaveManager saveManager = FindObjectOfType<SaveManager>();
        if (saveManager != null)
        {
            saveManager.DeleteSave(false); // Don't show message for new game
            Debug.Log("MainMenuManager: Save file deleted for new game");
        }
        else
        {
            // Fallback: Delete save file directly
            string saveFilePath = System.IO.Path.Combine(Application.persistentDataPath, "savegame.json");
            if (System.IO.File.Exists(saveFilePath))
            {
                System.IO.File.Delete(saveFilePath);
                Debug.Log("MainMenuManager: Save file deleted directly for new game");
            }
        }
        
        // Reset team for fresh start
        if (TeamManager.Instance != null)
        {
            TeamManager.Instance.ResetTeam();
            Debug.Log("MainMenuManager: Team reset for new game");
        }
        
        // Reset game progress (defeated trolls, etc.)
        GameProgress.ResetProgress();
        Debug.Log("MainMenuManager: Game progress reset for new game");
        
        // Load map scene directly (team will be empty, populated before first battle)
        SceneManager.LoadScene(mapSceneName);
    }
    
    public void OnLoadGameClicked()
    {
        Debug.Log("MainMenuManager: Load Game clicked - Checking for save file...");
        
        // Check if save file exists directly (more reliable than finding SaveManager)
        string saveFilePath = System.IO.Path.Combine(Application.persistentDataPath, "savegame.json");
        bool saveExists = System.IO.File.Exists(saveFilePath);
        
        Debug.Log($"MainMenuManager: Checking save file at: {saveFilePath}");
        Debug.Log($"MainMenuManager: Save file exists: {saveExists}");
        
        if (saveExists)
        {
            Debug.Log("MainMenuManager: Save file found - Loading game...");
            
            // Reset team before loading (since teams don't persist)
            if (TeamManager.Instance != null)
            {
                TeamManager.Instance.ResetTeam();
                Debug.Log("MainMenuManager: Team reset before loading saved game");
            }
            
            // Load the map scene directly - SaveManager will restore player state
            SceneManager.LoadScene(mapSceneName);
        }
        else
        {
            Debug.Log("MainMenuManager: No save file found - Starting new game instead...");
            // No save file, start new game
            OnNewGameClicked();
        }
    }
    
    public void OnSettingsClicked()
    {
        Debug.Log("MainMenuManager: Settings clicked - Opening settings panel...");
        ShowSettings();
    }
    
    public void OnQuitClicked()
    {
        Debug.Log("MainMenuManager: Quit clicked - Exiting game...");
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    
    public void OnBackToMenuClicked()
    {
        Debug.Log("MainMenuManager: Back to menu clicked - Returning to main menu...");
        ShowMainMenu();
    }
    
    private void ShowMainMenu()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }
    
    private void ShowSettings()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }
    
    // Audio Settings Methods
    public void OnMasterVolumeChanged(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("MasterVolume", value);
        PlayerPrefs.Save();
        Debug.Log($"MainMenuManager: Master volume changed to {value}");
    }
    
    public void OnMusicVolumeChanged(float value)
    {
        if (backgroundMusicSource != null)
        {
            backgroundMusicSource.volume = value;
        }
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
        Debug.Log($"MainMenuManager: Music volume changed to {value}");
    }
    
    public void OnSFXVolumeChanged(float value)
    {
        // This will be used by other audio sources in the game
        PlayerPrefs.SetFloat("SFXVolume", value);
        PlayerPrefs.Save();
        Debug.Log($"MainMenuManager: SFX volume changed to {value}");
    }
    
    // Public method to get current SFX volume (for other scripts)
    public static float GetSFXVolume()
    {
        return PlayerPrefs.GetFloat("SFXVolume", 1.0f);
    }
    
    // Public method to get current music volume (for other scripts)
    public static float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat("MusicVolume", 0.8f);
    }
}
