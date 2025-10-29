using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Simple GameOverManager - no complex setups needed
/// </summary>
public class GameOverManager : MonoBehaviour
{
    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverTitle;
    public TextMeshProUGUI defeatMessage;
    public TextMeshProUGUI statisticsText;
    public Image backgroundOverlay;

    [Header("Buttons")]
    public Button restartBattleButton;
    public Button mainMenuButton;
    public Button quitGameButton;

    [Header("Scene Names")]
    public string mainMenuSceneName = "MainMenuScene";

    void Start()
    {
        Debug.LogError("GameOverManager: Start() called");
        
        // Update statistics
        UpdateStatistics();
        
        // Setup buttons - simple and direct
        SetupButtons();
    }

    void UpdateStatistics()
    {
        int defeatedTrolls = GameProgress.GetDefeatedTrollCount();
        string battleZone = BattleData.battleZoneName;
        string cleanZoneName = GetCleanZoneName(battleZone);
        
        if (defeatMessage != null)
        {
            defeatMessage.text = $"Defeated in {cleanZoneName}";
        }
        
        if (statisticsText != null)
        {
            statisticsText.text = $"Trolls Defeated: {defeatedTrolls}/3\nBattle Location: {cleanZoneName}";
        }
    }

    string GetCleanZoneName(string originalName)
    {
        if (string.IsNullOrEmpty(originalName))
            return "Unknown Location";
            
        if (originalName.StartsWith("Top-Down Simple Summer_Prop - "))
        {
            return originalName.Replace("Top-Down Simple Summer_Prop - ", "");
        }
        
        if (originalName.Contains("Castle"))
            return "Round Castle";
        if (originalName.Contains("Tent"))
            return "Tent Camp";
        if (originalName.Contains("Forest"))
            return "Dark Forest";
            
        return originalName;
    }

    void SetupButtons()
    {
        Debug.LogError("GameOverManager: Setting up buttons...");
        
        // DIAGNOSTIC: Check EventSystem
        CheckEventSystem();
        
        // DIAGNOSTIC: Check Canvas
        CheckCanvas();
        
        // Restart Battle Button
        if (restartBattleButton != null)
        {
            restartBattleButton.onClick.RemoveAllListeners();
            restartBattleButton.onClick.AddListener(() => {
                Debug.LogError("GameOverManager: RESTART BUTTON CLICKED!");
                SceneManager.LoadScene("BattleScene");
            });
        }

        // Main Menu Button - SIMPLE AND DIRECT
        if (mainMenuButton != null)
        {
            Debug.LogError($"GameOverManager: Main menu button found - Interactable: {mainMenuButton.interactable}, Active: {mainMenuButton.gameObject.activeInHierarchy}");
            
            // DIAGNOSTIC: Check button components
            CheckButtonComponents(mainMenuButton);
            
            mainMenuButton.onClick.RemoveAllListeners();
            mainMenuButton.onClick.AddListener(() => {
                Debug.LogError("GameOverManager: MAIN MENU BUTTON CLICKED!");
                SceneManager.LoadScene(mainMenuSceneName);
            });
            
            // FORCE BUTTON TO BE CLICKABLE
            ForceButtonClickable(mainMenuButton);
            
            Debug.LogError("GameOverManager: Main menu button setup complete");
        }
        else
        {
            Debug.LogError("GameOverManager: mainMenuButton is NULL!");
        }

        // Quit Game Button
        if (quitGameButton != null)
        {
            quitGameButton.onClick.RemoveAllListeners();
            quitGameButton.onClick.AddListener(() => {
                Debug.LogError("GameOverManager: QUIT BUTTON CLICKED!");
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                #else
                    Application.Quit();
                #endif
            });
        }
    }

    void CheckEventSystem()
    {
        UnityEngine.EventSystems.EventSystem eventSystem = FindObjectOfType<UnityEngine.EventSystems.EventSystem>();
        if (eventSystem != null)
        {
            Debug.LogError($"GameOverManager: EventSystem found - Active: {eventSystem.gameObject.activeInHierarchy}, Enabled: {eventSystem.enabled}");
        }
        else
        {
            Debug.LogError("GameOverManager: NO EventSystem found! This will prevent UI clicks!");
        }
    }

    void CheckCanvas()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas != null)
        {
            Debug.LogError($"GameOverManager: Canvas found - RenderMode: {canvas.renderMode}, Active: {canvas.gameObject.activeInHierarchy}");
            
            CanvasGroup canvasGroup = canvas.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                Debug.LogError($"GameOverManager: CanvasGroup found - Interactable: {canvasGroup.interactable}, BlocksRaycasts: {canvasGroup.blocksRaycasts}");
            }
        }
        else
        {
            Debug.LogError("GameOverManager: NO Canvas found!");
        }
    }

    void CheckButtonComponents(Button button)
    {
        Debug.LogError($"GameOverManager: Checking button components for {button.name}:");
        
        // Check Image component
        Image image = button.GetComponent<Image>();
        if (image != null)
        {
            Debug.LogError($"GameOverManager: Image found - RaycastTarget: {image.raycastTarget}");
        }
        else
        {
            Debug.LogError("GameOverManager: NO Image component found on button!");
        }
        
        // Check Button component
        Debug.LogError($"GameOverManager: Button component - Enabled: {button.enabled}, Interactable: {button.interactable}");
        
        // Check if button is blocked by other UI elements
        RectTransform rectTransform = button.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            Debug.LogError($"GameOverManager: Button position - X: {rectTransform.position.x}, Y: {rectTransform.position.y}, Z: {rectTransform.position.z}");
        }
    }

    // Public methods for external scripts
    public void ShowGameOver(string battleZoneName, int defeatedTrollsCount)
    {
        UpdateStatistics();
    }

    public void ForceReinitialize()
    {
        UpdateStatistics();
        SetupButtons();
    }

    public void TestButtonClick()
    {
        Debug.LogError("GameOverManager: TEST BUTTON CLICKED - Script is working!");
    }

    public void OnMainMenuClicked()
    {
        Debug.LogError("GameOverManager: MAIN MENU BUTTON CLICKED!");
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void MainMenuButtonClicked()
    {
        Debug.LogError("GameOverManager: MAIN MENU BUTTON CLICKED!");
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void GoToMainMenu()
    {
        Debug.LogError("GameOverManager: MAIN MENU BUTTON CLICKED!");
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void LoadMainMenu()
    {
        Debug.LogError("GameOverManager: MAIN MENU BUTTON CLICKED!");
        SceneManager.LoadScene(mainMenuSceneName);
    }

    void ForceButtonClickable(Button button)
    {
        Debug.LogError($"GameOverManager: Forcing button {button.name} to be clickable...");
        
        // Ensure button is at the top of the hierarchy
        button.transform.SetAsLastSibling();
        Debug.LogError("GameOverManager: Button moved to top of hierarchy");
        
        // Ensure all parent objects are active and enabled
        Transform parent = button.transform.parent;
        while (parent != null)
        {
            if (!parent.gameObject.activeInHierarchy)
            {
                Debug.LogError($"GameOverManager: Activating parent: {parent.name}");
                parent.gameObject.SetActive(true);
            }
            parent = parent.parent;
        }
        
        // Force the button to be interactable
        button.interactable = true;
        button.enabled = true;
        
        // Ensure the Image component is set up correctly
        Image image = button.GetComponent<Image>();
        if (image != null)
        {
            image.raycastTarget = true;
            Debug.LogError("GameOverManager: Image raycast target enabled");
        }
        
        Debug.LogError("GameOverManager: Button clickability forced - ready for manual clicks!");
    }
}