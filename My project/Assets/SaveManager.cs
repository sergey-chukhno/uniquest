using UnityEngine;
using System.IO;
using System.Collections.Generic;

/// <summary>
/// Manages saving and loading game data
/// Uses JSON for human-readable save files
/// </summary>
public class SaveManager : MonoBehaviour
{
    [Header("Save Settings")]
    public string saveFileName = "savegame.json";
    
    [Header("References")]
    public PlayerController playerController;
    public Inventory playerInventory;
    public MessageDisplay messageDisplay;
    
    [Header("Auto-Save")]
    public bool autoSaveOnVictory = true;
    
    private string SaveFilePath => Path.Combine(Application.persistentDataPath, saveFileName);
    private float playTime = 0f;
    
    void Start()
    {
        // Find references if not assigned
        if (playerController == null)
        {
            playerController = FindObjectOfType<PlayerController>();
        }
        
        if (playerInventory == null)
        {
            playerInventory = FindObjectOfType<Inventory>();
        }
        
        if (messageDisplay == null)
        {
            messageDisplay = FindObjectOfType<MessageDisplay>();
        }
        
        // Auto-load on start if save exists
        // Use Invoke to ensure load happens after all other Start() methods
        if (SaveFileExists())
        {
            Invoke(nameof(LoadGame), 0.1f);
        }
        else
        {
            Debug.Log("No save file found - starting fresh game");
            ShowMessage("[NEW GAME] Starting new adventure!");
        }
    }
    
    void Update()
    {
        // Track play time
        playTime += Time.deltaTime;
        
        // Manual save with S key
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveGame();
        }
        
        // Manual load with L key
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadGame();
        }
        
        // Delete save with D key (for testing)
        if (Input.GetKeyDown(KeyCode.D))
        {
            DeleteSave();
        }
    }
    
    /// <summary>
    /// Save current game state to file
    /// </summary>
    public void SaveGame()
    {
        try
        {
            // Create save data object
            SaveData data = new SaveData();
            
            // Save player stats
            if (playerController != null)
            {
                data.playerHealth = playerController.currentHealth;
                data.playerMaxHealth = playerController.maxHealth;
                data.playerMana = playerController.currentMana;
                data.playerMaxMana = playerController.maxMana;
                
                // Save player position
                data.playerPositionX = playerController.transform.position.x;
                data.playerPositionY = playerController.transform.position.y;
            }
            
            // Save inventory
            if (playerInventory != null)
            {
                data.healthPotionCount = playerInventory.GetItemQuantity(ItemType.HealthPotion);
                data.manaPotionCount = playerInventory.GetItemQuantity(ItemType.ManaPotion);
            }
            
            // Save game progress (defeated trolls)
            data.defeatedTrolls = new List<int>();
            for (int i = 1; i <= 3; i++)
            {
                if (GameProgress.IsTrollDefeated(i))
                {
                    data.defeatedTrolls.Add(i);
                }
            }
            
            // Save collected objects
            if (playerController != null)
            {
                data.collectedObjects = new List<string>(playerController.GetCollectedObjects());
            }
            
            // Save metadata
            data.saveTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            data.playTime = playTime;
            
            // Convert to JSON
            string json = JsonUtility.ToJson(data, true);
            
            // Write to file
            File.WriteAllText(SaveFilePath, json);
            
            Debug.Log($"[SAVE] Game saved to: {SaveFilePath}");
            ShowMessage("[GAME SAVED] Progress saved successfully!");
            
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[SAVE ERROR] Failed to save game: {e.Message}");
            ShowMessage("[SAVE FAILED] Could not save game!");
        }
    }
    
    /// <summary>
    /// Load game state from file
    /// </summary>
    public void LoadGame()
    {
        try
        {
            if (!SaveFileExists())
            {
                Debug.Log("[LOAD] No save file found");
                ShowMessage("[NO SAVE] No save file found!");
                return;
            }
            
            // Read from file
            string json = File.ReadAllText(SaveFilePath);
            
            // Parse JSON
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            
            // Restore player stats
            if (playerController != null)
            {
                playerController.currentHealth = data.playerHealth;
                playerController.maxHealth = data.playerMaxHealth;
                playerController.currentMana = data.playerMana;
                playerController.maxMana = data.playerMaxMana;
                
                // Important: Mark stats as initialized to prevent Start() from resetting them
                // Use reflection to set the private field
                var field = typeof(PlayerController).GetField("statsInitialized", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (field != null)
                {
                    field.SetValue(playerController, true);
                }
                
                // Restore player position
                playerController.transform.position = new Vector3(
                    data.playerPositionX,
                    data.playerPositionY,
                    playerController.transform.position.z
                );
            }
            
            // Restore inventory
            if (playerInventory != null)
            {
                // Clear current inventory
                playerInventory.items.Clear();
                
                // Add saved items
                if (data.healthPotionCount > 0)
                {
                    playerInventory.AddItem(new Item(
                        "Health Potion",
                        "Restores 30 HP",
                        ItemType.HealthPotion,
                        30,
                        data.healthPotionCount
                    ));
                }
                
                if (data.manaPotionCount > 0)
                {
                    playerInventory.AddItem(new Item(
                        "Mana Potion",
                        "Restores 25 MP",
                        ItemType.ManaPotion,
                        25,
                        data.manaPotionCount
                    ));
                }
                
                // Mark inventory as initialized to prevent Start() from adding items again
                playerInventory.MarkAsInitialized();
            }
            
            // Restore game progress (defeated trolls)
            GameProgress.ResetProgress(); // Clear first
            foreach (int trollIndex in data.defeatedTrolls)
            {
                GameProgress.DefeatTroll(trollIndex);
            }
            
            // Restore collected objects
            if (playerController != null)
            {
                playerController.SetCollectedObjects(data.collectedObjects);
            }
            
            // Restore play time
            playTime = data.playTime;
            
            Debug.Log($"[LOAD] Game loaded from: {SaveFilePath}");
            Debug.Log($"[LOAD] Player at ({data.playerPositionX}, {data.playerPositionY})");
            Debug.Log($"[LOAD] HP: {data.playerHealth}/{data.playerMaxHealth}, MP: {data.playerMana}/{data.playerMaxMana}");
            Debug.Log($"[LOAD] Defeated trolls: {data.defeatedTrolls.Count}/3");
            
            ShowMessage($"[GAME LOADED] Progress restored! ({data.defeatedTrolls.Count}/3 trolls defeated)");
            
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[LOAD ERROR] Failed to load game: {e.Message}");
            ShowMessage("[LOAD FAILED] Could not load game!");
        }
    }
    
    /// <summary>
    /// Auto-save after battle victory
    /// </summary>
    public void AutoSave()
    {
        if (autoSaveOnVictory)
        {
            SaveGame();
            Debug.Log("[AUTO-SAVE] Game auto-saved after battle victory");
        }
    }
    
    /// <summary>
    /// Delete save file (for testing)
    /// </summary>
    public void DeleteSave()
    {
        try
        {
            if (SaveFileExists())
            {
                File.Delete(SaveFilePath);
                Debug.Log($"[DELETE] Save file deleted: {SaveFilePath}");
                ShowMessage("[SAVE DELETED] Save file removed! Restart for fresh game.");
            }
            else
            {
                Debug.Log("[DELETE] No save file to delete");
                ShowMessage("[NO SAVE] No save file to delete!");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[DELETE ERROR] Failed to delete save: {e.Message}");
        }
    }
    
    /// <summary>
    /// Check if save file exists
    /// </summary>
    public bool SaveFileExists()
    {
        return File.Exists(SaveFilePath);
    }
    
    /// <summary>
    /// Get save file path for debugging
    /// </summary>
    public string GetSaveFilePath()
    {
        return SaveFilePath;
    }
    
    /// <summary>
    /// Show message on screen
    /// </summary>
    private void ShowMessage(string message)
    {
        if (messageDisplay != null)
        {
            messageDisplay.ShowMessage(message);
        }
        Debug.Log(message);
    }
}

