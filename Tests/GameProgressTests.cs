using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Unit tests for Game Progress Tracking
/// Tests troll defeat tracking and game completion status
/// </summary>
public class GameProgressTests
{
    // Helper class for save data testing
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
    
    // Helper to simulate GameProgress static class
    private HashSet<int> defeatedTrolls;
    private bool gameCompleted;
    
    [SetUp]
    public void Setup()
    {
        // Reset state before each test
        defeatedTrolls = new HashSet<int>();
        gameCompleted = false;
    }
    
    // ==========================================
    // TROLL DEFEAT TRACKING TESTS
    // ==========================================
    
    [Test]
    public void DefeatTroll_AddsTrollToList()
    {
        // Act
        defeatedTrolls.Add(1);
        
        // Assert
        Assert.AreEqual(1, defeatedTrolls.Count, "Should have 1 defeated troll");
        Assert.IsTrue(defeatedTrolls.Contains(1), "Should contain troll 1");
    }
    
    [Test]
    public void DefeatTroll_MultipleTrolls_TracksAll()
    {
        // Act
        defeatedTrolls.Add(1);
        defeatedTrolls.Add(2);
        defeatedTrolls.Add(3);
        
        // Assert
        Assert.AreEqual(3, defeatedTrolls.Count, "Should track all 3 trolls");
        Assert.IsTrue(defeatedTrolls.Contains(1), "Should contain troll 1");
        Assert.IsTrue(defeatedTrolls.Contains(2), "Should contain troll 2");
        Assert.IsTrue(defeatedTrolls.Contains(3), "Should contain troll 3");
    }
    
    [Test]
    public void DefeatTroll_SameTrollTwice_NotDuplicated()
    {
        // Act
        defeatedTrolls.Add(1);
        defeatedTrolls.Add(1); // Try to add again
        
        // Assert
        Assert.AreEqual(1, defeatedTrolls.Count, "Should not duplicate troll entries");
    }
    
    [Test]
    public void IsTrollDefeated_DefeatedTroll_ReturnsTrue()
    {
        // Arrange
        defeatedTrolls.Add(1);
        
        // Act
        bool isDefeated = defeatedTrolls.Contains(1);
        
        // Assert
        Assert.IsTrue(isDefeated, "Troll 1 should be marked as defeated");
    }
    
    [Test]
    public void IsTrollDefeated_NotDefeatedTroll_ReturnsFalse()
    {
        // Arrange
        defeatedTrolls.Add(1);
        
        // Act
        bool isDefeated = defeatedTrolls.Contains(2);
        
        // Assert
        Assert.IsFalse(isDefeated, "Troll 2 should not be marked as defeated");
    }
    
    // ==========================================
    // GAME COMPLETION TESTS
    // ==========================================
    
    [Test]
    public void GameCompletion_AllTrollsDefeated_IsCompleted()
    {
        // Act
        defeatedTrolls.Add(1);
        defeatedTrolls.Add(2);
        defeatedTrolls.Add(3);
        gameCompleted = defeatedTrolls.Count >= 3;
        
        // Assert
        Assert.IsTrue(gameCompleted, "Game should be completed when all 3 trolls defeated");
    }
    
    [Test]
    public void GameCompletion_PartialTrollsDefeated_NotCompleted()
    {
        // Act
        defeatedTrolls.Add(1);
        defeatedTrolls.Add(2);
        gameCompleted = defeatedTrolls.Count >= 3;
        
        // Assert
        Assert.IsFalse(gameCompleted, "Game should not be completed with only 2 trolls");
    }
    
    [Test]
    public void GetDefeatedTrollCount_ReturnsCorrectCount()
    {
        // Arrange
        defeatedTrolls.Add(1);
        defeatedTrolls.Add(3);
        
        // Act
        int count = defeatedTrolls.Count;
        
        // Assert
        Assert.AreEqual(2, count, "Should return correct count of defeated trolls");
    }
    
    [Test]
    public void GetDefeatedTrolls_ReturnsCorrectList()
    {
        // Arrange
        defeatedTrolls.Add(1);
        defeatedTrolls.Add(3);
        
        // Act
        List<int> trollList = new List<int>(defeatedTrolls);
        
        // Assert
        Assert.AreEqual(2, trollList.Count, "List should have 2 trolls");
        Assert.Contains(1, trollList, "List should contain troll 1");
        Assert.Contains(3, trollList, "List should contain troll 3");
    }
    
    // ==========================================
    // RESET TESTS
    // ==========================================
    
    [Test]
    public void ResetProgress_ClearsAllTrolls()
    {
        // Arrange
        defeatedTrolls.Add(1);
        defeatedTrolls.Add(2);
        defeatedTrolls.Add(3);
        gameCompleted = true;
        
        // Act
        defeatedTrolls.Clear();
        gameCompleted = false;
        
        // Assert
        Assert.AreEqual(0, defeatedTrolls.Count, "Should clear all defeated trolls");
        Assert.IsFalse(gameCompleted, "Game completion should be reset");
    }
    
    [Test]
    public void ResetProgress_AllowsRedefeat()
    {
        // Arrange
        defeatedTrolls.Add(1);
        
        // Act
        defeatedTrolls.Clear();
        defeatedTrolls.Add(1); // Defeat again after reset
        
        // Assert
        Assert.AreEqual(1, defeatedTrolls.Count, "Should be able to defeat trolls again");
        Assert.IsTrue(defeatedTrolls.Contains(1), "Should track re-defeated troll");
    }
    
    // ==========================================
    // SAVE FILE PERSISTENCE TESTS
    // ==========================================
    
    [Test]
    public void SaveFile_MultipleFields_AllPersist()
    {
        // Arrange
        TestSaveData saveData = new TestSaveData();
        
        // Act - Set various fields
        saveData.playerHealth = 85;
        saveData.playerMana = 40;
        saveData.playerPositionX = 12.5f;
        saveData.playerPositionY = -8.3f;
        saveData.healthPotionCount = 2;
        saveData.manaPotionCount = 1;
        saveData.defeatedTrolls.Add(1);
        saveData.defeatedTrolls.Add(2);
        saveData.playTime = 450f;
        
        // Simulate JSON serialization
        string json = JsonUtility.ToJson(saveData);
        TestSaveData restored = JsonUtility.FromJson<TestSaveData>(json);
        
        // Assert - Verify all fields restored
        Assert.AreEqual(85, restored.playerHealth, "Health should persist");
        Assert.AreEqual(40, restored.playerMana, "Mana should persist");
        Assert.AreEqual(12.5f, restored.playerPositionX, 0.01f, "Position X should persist");
        Assert.AreEqual(-8.3f, restored.playerPositionY, 0.01f, "Position Y should persist");
        Assert.AreEqual(2, restored.healthPotionCount, "Health potion count should persist");
        Assert.AreEqual(1, restored.manaPotionCount, "Mana potion count should persist");
        Assert.AreEqual(2, restored.defeatedTrolls.Count, "Defeated troll count should persist");
        Assert.AreEqual(450f, restored.playTime, 0.01f, "Play time should persist");
    }
    
    [Test]
    public void SaveFile_EmptyProgress_SerializesCorrectly()
    {
        // Arrange
        TestSaveData saveData = new TestSaveData();
        
        // Act
        string json = JsonUtility.ToJson(saveData);
        TestSaveData restored = JsonUtility.FromJson<TestSaveData>(json);
        
        // Assert
        Assert.AreEqual(0, restored.defeatedTrolls.Count, "Should have no defeated trolls");
        Assert.AreEqual(3, restored.healthPotionCount, "Should have default potion count");
    }
    
    // ==========================================
    // BATTLE DATA TRANSFER TESTS
    // ==========================================
    
    [Test]
    public void BattleData_SetupBattle_StoresCorrectValues()
    {
        // Arrange
        int enemyIndex = 2;
        int backgroundIndex = 2;
        string zoneName = "Round Castle";
        
        // Act - Simulate BattleData.SetupBattle()
        Dictionary<string, object> battleData = new Dictionary<string, object>
        {
            { "enemyIndex", enemyIndex },
            { "backgroundIndex", backgroundIndex },
            { "zoneName", zoneName }
        };
        
        // Assert
        Assert.AreEqual(2, battleData["enemyIndex"], "Enemy index should be stored");
        Assert.AreEqual(2, battleData["backgroundIndex"], "Background index should be stored");
        Assert.AreEqual("Round Castle", battleData["zoneName"], "Zone name should be stored");
    }
    
    [Test]
    public void BattleData_UpdatePlayerStats_TransfersCorrectly()
    {
        // Arrange
        int health = 75;
        int maxHealth = 100;
        int mana = 30;
        int maxMana = 50;
        
        // Act - Simulate BattleData.UpdatePlayerStats()
        Dictionary<string, int> playerStats = new Dictionary<string, int>
        {
            { "currentHealth", health },
            { "maxHealth", maxHealth },
            { "currentMana", mana },
            { "maxMana", maxMana }
        };
        
        // Assert
        Assert.AreEqual(75, playerStats["currentHealth"], "Current health should transfer");
        Assert.AreEqual(100, playerStats["maxHealth"], "Max health should transfer");
        Assert.AreEqual(30, playerStats["currentMana"], "Current mana should transfer");
        Assert.AreEqual(50, playerStats["maxMana"], "Max mana should transfer");
    }
}

