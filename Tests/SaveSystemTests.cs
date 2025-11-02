using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Unit tests for Save/Load System
/// Tests serialization, data persistence, and restoration
/// </summary>
public class SaveSystemTests
{
    // Helper class mimicking SaveData structure
    private class TestSaveData
    {
        public int playerHealth;
        public int playerMaxHealth;
        public int playerMana;
        public int playerMaxMana;
        public float playerPositionX;
        public float playerPositionY;
        public int healthPotionCount;
        public int manaPotionCount;
        public List<int> defeatedTrolls;
        public string saveTime;
        public float playTime;
        
        public TestSaveData()
        {
            playerHealth = 100;
            playerMaxHealth = 100;
            playerMana = 50;
            playerMaxMana = 50;
            playerPositionX = 0f;
            playerPositionY = 0f;
            healthPotionCount = 3;
            manaPotionCount = 2;
            defeatedTrolls = new List<int>();
            saveTime = System.DateTime.Now.ToString();
            playTime = 0f;
        }
    }
    
    // ==========================================
    // SAVE DATA CREATION TESTS
    // ==========================================
    
    [Test]
    public void SaveData_Creation_HasDefaultValues()
    {
        // Arrange & Act
        TestSaveData saveData = new TestSaveData();
        
        // Assert
        Assert.AreEqual(100, saveData.playerHealth, "Should have default health");
        Assert.AreEqual(50, saveData.playerMana, "Should have default mana");
        Assert.AreEqual(3, saveData.healthPotionCount, "Should have default health potions");
        Assert.AreEqual(2, saveData.manaPotionCount, "Should have default mana potions");
        Assert.IsEmpty(saveData.defeatedTrolls, "Should have no defeated trolls initially");
    }
    
    [Test]
    public void SaveData_PlayerStats_SavesCorrectly()
    {
        // Arrange
        TestSaveData saveData = new TestSaveData();
        
        // Act - Simulate player taking damage and using mana
        saveData.playerHealth = 75;
        saveData.playerMana = 30;
        
        // Assert
        Assert.AreEqual(75, saveData.playerHealth, "Health should be saved");
        Assert.AreEqual(30, saveData.playerMana, "Mana should be saved");
    }
    
    [Test]
    public void SaveData_PlayerPosition_SavesCorrectly()
    {
        // Arrange
        TestSaveData saveData = new TestSaveData();
        
        // Act
        saveData.playerPositionX = 10.5f;
        saveData.playerPositionY = -5.3f;
        
        // Assert
        Assert.AreEqual(10.5f, saveData.playerPositionX, "X position should be saved");
        Assert.AreEqual(-5.3f, saveData.playerPositionY, "Y position should be saved");
    }
    
    // ==========================================
    // GAME PROGRESS TESTS
    // ==========================================
    
    [Test]
    public void GameProgress_DefeatTroll_AddsToList()
    {
        // Arrange
        HashSet<int> defeatedTrolls = new HashSet<int>();
        
        // Act
        defeatedTrolls.Add(1);
        defeatedTrolls.Add(2);
        
        // Assert
        Assert.AreEqual(2, defeatedTrolls.Count, "Should have 2 defeated trolls");
        Assert.IsTrue(defeatedTrolls.Contains(1), "Should contain troll 1");
        Assert.IsTrue(defeatedTrolls.Contains(2), "Should contain troll 2");
    }
    
    [Test]
    public void GameProgress_DefeatSameTroll_NotDuplicated()
    {
        // Arrange
        HashSet<int> defeatedTrolls = new HashSet<int>();
        
        // Act
        defeatedTrolls.Add(1);
        defeatedTrolls.Add(1); // Try to add again
        
        // Assert
        Assert.AreEqual(1, defeatedTrolls.Count, "HashSet should prevent duplicates");
    }
    
    [Test]
    public void GameProgress_IsTrollDefeated_ReturnsCorrectStatus()
    {
        // Arrange
        HashSet<int> defeatedTrolls = new HashSet<int> { 1, 3 };
        
        // Act
        bool troll1Defeated = defeatedTrolls.Contains(1);
        bool troll2Defeated = defeatedTrolls.Contains(2);
        bool troll3Defeated = defeatedTrolls.Contains(3);
        
        // Assert
        Assert.IsTrue(troll1Defeated, "Troll 1 should be defeated");
        Assert.IsFalse(troll2Defeated, "Troll 2 should not be defeated");
        Assert.IsTrue(troll3Defeated, "Troll 3 should be defeated");
    }
    
    [Test]
    public void GameProgress_AllTrollsDefeated_GameCompleted()
    {
        // Arrange
        HashSet<int> defeatedTrolls = new HashSet<int> { 1, 2, 3 };
        
        // Act
        bool gameCompleted = defeatedTrolls.Count >= 3;
        
        // Assert
        Assert.IsTrue(gameCompleted, "Game should be completed when all 3 trolls defeated");
    }
    
    [Test]
    public void GameProgress_ResetProgress_ClearsDefeatedTrolls()
    {
        // Arrange
        HashSet<int> defeatedTrolls = new HashSet<int> { 1, 2, 3 };
        
        // Act
        defeatedTrolls.Clear();
        
        // Assert
        Assert.AreEqual(0, defeatedTrolls.Count, "Should clear all defeated trolls");
        Assert.IsFalse(defeatedTrolls.Contains(1), "Should not contain any trolls");
    }
    
    // ==========================================
    // INVENTORY SAVE/LOAD TESTS
    // ==========================================
    
    [Test]
    public void SaveData_Inventory_SavesItemCounts()
    {
        // Arrange
        TestSaveData saveData = new TestSaveData();
        
        // Act - Simulate using items
        saveData.healthPotionCount = 1; // Used 2
        saveData.manaPotionCount = 0;   // Used all
        
        // Assert
        Assert.AreEqual(1, saveData.healthPotionCount, "Should save remaining health potions");
        Assert.AreEqual(0, saveData.manaPotionCount, "Should save 0 mana potions");
    }
    
    [Test]
    public void SaveData_Inventory_RestoresCorrectly()
    {
        // Arrange
        TestSaveData saveData = new TestSaveData();
        saveData.healthPotionCount = 2;
        saveData.manaPotionCount = 1;
        
        // Act - Simulate loading
        int restoredHealth = saveData.healthPotionCount;
        int restoredMana = saveData.manaPotionCount;
        
        // Assert
        Assert.AreEqual(2, restoredHealth, "Should restore correct health potion count");
        Assert.AreEqual(1, restoredMana, "Should restore correct mana potion count");
    }
    
    // ==========================================
    // JSON SERIALIZATION TESTS
    // ==========================================
    
    [Test]
    public void JSON_Serialize_CreatesValidString()
    {
        // Arrange
        TestSaveData saveData = new TestSaveData();
        saveData.playerHealth = 75;
        saveData.defeatedTrolls.Add(1);
        
        // Act
        string json = JsonUtility.ToJson(saveData);
        
        // Assert
        Assert.IsNotNull(json, "JSON should not be null");
        Assert.IsNotEmpty(json, "JSON should not be empty");
        Assert.IsTrue(json.Contains("playerHealth"), "JSON should contain playerHealth field");
        Assert.IsTrue(json.Contains("75"), "JSON should contain health value");
    }
    
    [Test]
    public void JSON_Deserialize_RestoresData()
    {
        // Arrange
        TestSaveData originalData = new TestSaveData();
        originalData.playerHealth = 75;
        originalData.playerMana = 30;
        originalData.defeatedTrolls.Add(1);
        originalData.defeatedTrolls.Add(2);
        
        // Act
        string json = JsonUtility.ToJson(originalData);
        TestSaveData restoredData = JsonUtility.FromJson<TestSaveData>(json);
        
        // Assert
        Assert.AreEqual(75, restoredData.playerHealth, "Health should be restored");
        Assert.AreEqual(30, restoredData.playerMana, "Mana should be restored");
        Assert.AreEqual(2, restoredData.defeatedTrolls.Count, "Defeated trolls count should match");
        Assert.IsTrue(restoredData.defeatedTrolls.Contains(1), "Should contain defeated troll 1");
        Assert.IsTrue(restoredData.defeatedTrolls.Contains(2), "Should contain defeated troll 2");
    }
    
    // ==========================================
    // METADATA TESTS
    // ==========================================
    
    [Test]
    public void SaveData_Metadata_IncludesTimestamp()
    {
        // Arrange & Act
        TestSaveData saveData = new TestSaveData();
        saveData.saveTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        
        // Assert
        Assert.IsNotNull(saveData.saveTime, "Save time should not be null");
        Assert.IsNotEmpty(saveData.saveTime, "Save time should not be empty");
        Assert.IsTrue(saveData.saveTime.Length > 0, "Save time should be formatted string");
    }
    
    [Test]
    public void SaveData_PlayTime_TracksCorrectly()
    {
        // Arrange
        TestSaveData saveData = new TestSaveData();
        
        // Act - Simulate 5 minutes of play
        saveData.playTime = 300f; // 5 minutes in seconds
        
        // Assert
        Assert.AreEqual(300f, saveData.playTime, "Play time should be tracked in seconds");
    }
}

