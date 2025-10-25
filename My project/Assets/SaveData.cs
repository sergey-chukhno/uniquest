using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data structure for saving game state
/// All game data that needs to persist between sessions goes here
/// </summary>
[System.Serializable]
public class SaveData
{
    [Header("Player Stats")]
    public int playerHealth = 100;
    public int playerMaxHealth = 100;
    public int playerMana = 50;
    public int playerMaxMana = 50;
    
    [Header("Player Position")]
    public float playerPositionX = 0f;
    public float playerPositionY = 0f;
    
    [Header("Inventory")]
    public int healthPotionCount = 3;
    public int manaPotionCount = 2;
    
    [Header("Game Progress")]
    public List<int> defeatedTrolls = new List<int>();
    
    [Header("Collected Objects")]
    public List<string> collectedObjects = new List<string>();
    
    [Header("Save Metadata")]
    public string saveTime = "";
    public float playTime = 0f;
    
    /// <summary>
    /// Constructor - creates a new save with default values
    /// </summary>
    public SaveData()
    {
        // Default starting values
        playerHealth = 100;
        playerMaxHealth = 100;
        playerMana = 50;
        playerMaxMana = 50;
        healthPotionCount = 3;
        manaPotionCount = 2;
        defeatedTrolls = new List<int>();
        collectedObjects = new List<string>();
        saveTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        playTime = 0f;
    }
}


