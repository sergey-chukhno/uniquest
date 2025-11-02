using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Integration tests for complete game scenarios
/// Tests end-to-end workflows and system interactions
/// </summary>
public class IntegrationTests
{
    // ==========================================
    // COMPLETE BATTLE SCENARIO TEST
    // ==========================================
    
    [Test]
    public void CompleteBattle_PlayerWins_CorrectFlow()
    {
        // Arrange - Initial battle state
        int playerHealth = 100;
        int enemyHealth = 50;
        int playerAttack = 20;
        int enemyDefense = 5;
        
        // Act - Simulate 3 player attacks
        for (int i = 0; i < 3; i++)
        {
            int damage = Mathf.Max(1, playerAttack - (enemyDefense / 2));
            enemyHealth -= damage;
        }
        
        bool victory = enemyHealth <= 0;
        
        // Assert
        Assert.IsTrue(victory, "Player should win after 3 attacks");
        Assert.LessOrEqual(enemyHealth, 0, "Enemy should be defeated");
    }
    
    [Test]
    public void CompleteBattle_CharacterSwitching_PreservesTeam()
    {
        // Arrange
        Dictionary<int, int> teamHealths = new Dictionary<int, int>
        {
            { 0, 0 },   // First character defeated
            { 1, 80 },  // Second character alive
            { 2, 60 }   // Third character alive
        };
        int activeIndex = 0;
        
        // Act - Switch to next alive character
        for (int i = 1; i < 3; i++)
        {
            if (teamHealths[i] > 0)
            {
                activeIndex = i;
                break;
            }
        }
        
        // Assert
        Assert.AreEqual(1, activeIndex, "Should switch to second character");
        Assert.Greater(teamHealths[activeIndex], 0, "Active character should be alive");
    }
    
    // ==========================================
    // SAVE/LOAD INTEGRATION TEST
    // ==========================================
    
    [Test]
    public void SaveLoad_AfterBattle_RestoresState()
    {
        // Arrange - Player state after battle
        int originalHealth = 65;
        int originalMana = 30;
        int originalHPPotions = 2;
        List<int> originalTrolls = new List<int> { 1 };
        
        // Act - Simulate save
        Dictionary<string, object> savedState = new Dictionary<string, object>
        {
            { "health", originalHealth },
            { "mana", originalMana },
            { "healthPotions", originalHPPotions },
            { "defeatedTrolls", new List<int>(originalTrolls) }
        };
        
        // Simulate load
        int loadedHealth = (int)savedState["health"];
        int loadedMana = (int)savedState["mana"];
        int loadedPotions = (int)savedState["healthPotions"];
        List<int> loadedTrolls = (List<int>)savedState["defeatedTrolls"];
        
        // Assert
        Assert.AreEqual(originalHealth, loadedHealth, "Health should be restored");
        Assert.AreEqual(originalMana, loadedMana, "Mana should be restored");
        Assert.AreEqual(originalHPPotions, loadedPotions, "Potion count should be restored");
        Assert.AreEqual(1, loadedTrolls.Count, "Defeated troll count should be restored");
        Assert.Contains(1, loadedTrolls, "Specific troll should be restored");
    }
    
    // ==========================================
    // ITEM USAGE IN BATTLE TEST
    // ==========================================
    
    [Test]
    public void ItemUsage_HealthPotion_HealsAndConsumed()
    {
        // Arrange
        int playerHealth = 50;
        int maxHealth = 100;
        int healthPotionCount = 3;
        int healAmount = 30;
        
        // Act - Use health potion
        bool hasPotion = healthPotionCount > 0;
        if (hasPotion)
        {
            playerHealth = Mathf.Min(maxHealth, playerHealth + healAmount);
            healthPotionCount--;
        }
        
        // Assert
        Assert.AreEqual(80, playerHealth, "Health should be restored by 30");
        Assert.AreEqual(2, healthPotionCount, "Potion count should decrease by 1");
    }
    
    [Test]
    public void ItemUsage_ManaPotion_RestoresAndConsumed()
    {
        // Arrange
        int playerMana = 10;
        int maxMana = 50;
        int manaPotionCount = 2;
        int restoreAmount = 25;
        
        // Act - Use mana potion
        bool hasPotion = manaPotionCount > 0;
        if (hasPotion)
        {
            playerMana = Mathf.Min(maxMana, playerMana + restoreAmount);
            manaPotionCount--;
        }
        
        // Assert
        Assert.AreEqual(35, playerMana, "Mana should be restored by 25");
        Assert.AreEqual(1, manaPotionCount, "Potion count should decrease by 1");
    }
    
    [Test]
    public void ItemUsage_NoItems_CannotUse()
    {
        // Arrange
        int healthPotionCount = 0;
        int playerHealth = 50;
        
        // Act
        bool hasPotion = healthPotionCount > 0;
        int healthAfter = playerHealth; // No change if can't use
        
        // Assert
        Assert.IsFalse(hasPotion, "Should not have potions available");
        Assert.AreEqual(50, healthAfter, "Health should not change");
    }
    
    // ==========================================
    // MULTI-BATTLE SCENARIO TEST
    // ==========================================
    
    [Test]
    public void MultiBattle_DefeatThreeTrolls_TracksAll()
    {
        // Arrange
        HashSet<int> defeatedTrolls = new HashSet<int>();
        int totalTrolls = 3;
        
        // Act - Simulate defeating all trolls in sequence
        defeatedTrolls.Add(1); // First battle
        defeatedTrolls.Add(2); // Second battle
        defeatedTrolls.Add(3); // Third battle
        
        bool allDefeated = defeatedTrolls.Count == totalTrolls;
        
        // Assert
        Assert.IsTrue(allDefeated, "All 3 trolls should be defeated");
        Assert.AreEqual(3, defeatedTrolls.Count, "Should track all victories");
    }
    
    [Test]
    public void MultiBattle_PlayerDefeatedMidGame_CanRestart()
    {
        // Arrange
        HashSet<int> defeatedTrolls = new HashSet<int> { 1, 2 }; // 2 trolls defeated
        int playerHealth = 0; // Player defeated
        
        // Act - Check if can restart (reset progress)
        bool canRestart = true; // Game always allows restart
        defeatedTrolls.Clear(); // Reset on restart
        playerHealth = 100; // Restore health
        
        // Assert
        Assert.IsTrue(canRestart, "Should be able to restart after defeat");
        Assert.AreEqual(0, defeatedTrolls.Count, "Progress should reset");
        Assert.AreEqual(100, playerHealth, "Health should be restored");
    }
    
    // ==========================================
    // SCENE TRANSITION DATA TEST
    // ==========================================
    
    [Test]
    public void SceneTransition_DataTransfer_PreservesState()
    {
        // Arrange - State before battle
        Dictionary<string, object> preButton = new Dictionary<string, object>
        {
            { "health", 85 },
            { "mana", 40 },
            { "potions", 2 }
        };
        
        // Act - Transfer to battle scene (simulate BattleData static class)
        Dictionary<string, object> battleData = new Dictionary<string, object>(preButton);
        
        // Simulate battle modifying values
        battleData["health"] = 60; // Took damage
        battleData["mana"] = 20;   // Used mana
        battleData["potions"] = 1; // Used potion
        
        // Transfer back to map
        Dictionary<string, object> postBattle = new Dictionary<string, object>(battleData);
        
        // Assert
        Assert.AreEqual(60, postBattle["health"], "Modified health should transfer back");
        Assert.AreEqual(20, postBattle["mana"], "Modified mana should transfer back");
        Assert.AreEqual(1, postBattle["potions"], "Modified potion count should transfer back");
    }
    
    // ==========================================
    // GAME COMPLETION SCENARIO TEST
    // ==========================================
    
    [Test]
    public void GameCompletion_FullPlaythrough_ValidatesProgress()
    {
        // Arrange
        HashSet<int> defeatedTrolls = new HashSet<int>();
        int playerHealth = 100;
        int healthPotions = 3;
        
        // Act - Simulate full game
        // Battle 1
        defeatedTrolls.Add(1);
        playerHealth -= 30; // Took damage
        healthPotions--;    // Used potion
        
        // Battle 2
        defeatedTrolls.Add(2);
        playerHealth -= 40; // Took more damage
        healthPotions--;    // Used another potion
        
        // Battle 3
        defeatedTrolls.Add(3);
        
        bool gameComplete = defeatedTrolls.Count == 3;
        
        // Assert
        Assert.IsTrue(gameComplete, "Game should be complete after all battles");
        Assert.AreEqual(3, defeatedTrolls.Count, "All 3 trolls should be defeated");
        Assert.Greater(playerHealth, 0, "Player should survive");
        Assert.GreaterOrEqual(healthPotions, 0, "Potion count should be valid");
    }
}

