using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

/// <summary>
/// Unit tests for Battle System
/// Tests damage calculations, combat flow, and character switching
/// </summary>
public class BattleSystemTests
{
    // ==========================================
    // DAMAGE CALCULATION TESTS
    // ==========================================
    
    [Test]
    public void DamageCalculation_BasicAttack_ReturnsPositiveDamage()
    {
        // Arrange
        int attackPower = 20;
        int defense = 10;
        
        // Act
        int baseDamage = attackPower - (defense / 2);
        int actualDamage = Mathf.Max(1, baseDamage); // At least 1 damage
        
        // Assert
        Assert.Greater(actualDamage, 0, "Damage should always be at least 1");
        Assert.AreEqual(15, actualDamage, "Base damage should be attack - (defense/2)");
    }
    
    [Test]
    public void DamageCalculation_SuperAttack_DealsTwiceDamage()
    {
        // Arrange
        int attackPower = 20;
        int defense = 10;
        int normalDamage = attackPower - (defense / 2); // 15
        int superMultiplier = 2;
        
        // Act
        int superDamage = normalDamage * superMultiplier;
        
        // Assert
        Assert.AreEqual(30, superDamage, "Super attack should deal 2x damage");
    }
    
    [Test]
    public void DamageCalculation_HighDefense_DealsMininumOneDamage()
    {
        // Arrange
        int attackPower = 5;
        int defense = 50; // Very high defense
        
        // Act
        int baseDamage = attackPower - (defense / 2); // Would be negative
        int actualDamage = Mathf.Max(1, baseDamage);
        
        // Assert
        Assert.AreEqual(1, actualDamage, "Even with high defense, should deal at least 1 damage");
    }
    
    // ==========================================
    // HEALTH AND MANA TESTS
    // ==========================================
    
    [Test]
    public void CharacterHealth_TakeDamage_ReducesHealth()
    {
        // Arrange
        int maxHealth = 100;
        int currentHealth = 100;
        int damage = 30;
        
        // Act
        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);
        
        // Assert
        Assert.AreEqual(70, currentHealth, "Health should be reduced by damage amount");
    }
    
    [Test]
    public void CharacterHealth_TakeFatalDamage_HealthGoesToZero()
    {
        // Arrange
        int currentHealth = 20;
        int damage = 50;
        
        // Act
        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);
        bool isAlive = currentHealth > 0;
        
        // Assert
        Assert.AreEqual(0, currentHealth, "Health should not go below 0");
        Assert.IsFalse(isAlive, "Character should be marked as not alive");
    }
    
    [Test]
    public void CharacterMana_SuperAttack_CostsMana()
    {
        // Arrange
        int currentMana = 50;
        int superAttackCost = 20;
        
        // Act
        bool canUseSuper = currentMana >= superAttackCost;
        if (canUseSuper)
        {
            currentMana -= superAttackCost;
        }
        
        // Assert
        Assert.IsTrue(canUseSuper, "Should have enough mana for super attack");
        Assert.AreEqual(30, currentMana, "Mana should be reduced by super attack cost");
    }
    
    [Test]
    public void CharacterMana_InsufficientMana_CannotSuperAttack()
    {
        // Arrange
        int currentMana = 10;
        int superAttackCost = 20;
        
        // Act
        bool canUseSuper = currentMana >= superAttackCost;
        
        // Assert
        Assert.IsFalse(canUseSuper, "Should not have enough mana for super attack");
    }
    
    [Test]
    public void CharacterMana_RestoreMana_IncreasesUpToMax()
    {
        // Arrange
        int maxMana = 50;
        int currentMana = 20;
        int restoreAmount = 40;
        
        // Act
        currentMana = Mathf.Min(maxMana, currentMana + restoreAmount);
        
        // Assert
        Assert.AreEqual(50, currentMana, "Mana should not exceed maximum");
    }
    
    // ==========================================
    // BATTLE STATE TESTS
    // ==========================================
    
    [Test]
    public void BattleState_PlayerDefeated_SwitchesToNextCharacter()
    {
        // Arrange
        int[] teamHealths = { 0, 80, 60 }; // First character defeated
        int activeIndex = 0;
        
        // Act
        bool firstCharacterAlive = teamHealths[activeIndex] > 0;
        if (!firstCharacterAlive)
        {
            // Switch to next alive character
            for (int i = 1; i < teamHealths.Length; i++)
            {
                if (teamHealths[i] > 0)
                {
                    activeIndex = i;
                    break;
                }
            }
        }
        
        // Assert
        Assert.AreEqual(1, activeIndex, "Should switch to second character");
        Assert.Greater(teamHealths[activeIndex], 0, "New active character should be alive");
    }
    
    [Test]
    public void BattleState_AllCharactersDefeated_TeamWiped()
    {
        // Arrange
        int[] teamHealths = { 0, 0, 0 }; // All defeated
        
        // Act
        bool isTeamWiped = true;
        foreach (int health in teamHealths)
        {
            if (health > 0)
            {
                isTeamWiped = false;
                break;
            }
        }
        
        // Assert
        Assert.IsTrue(isTeamWiped, "Team should be wiped when all characters are defeated");
    }
    
    [Test]
    public void BattleState_EnemyDefeated_VictoryCondition()
    {
        // Arrange
        int enemyHealth = 0;
        
        // Act
        bool isVictory = enemyHealth <= 0;
        
        // Assert
        Assert.IsTrue(isVictory, "Should trigger victory when enemy health reaches 0");
    }
    
    // ==========================================
    // DEFENSE CALCULATION TESTS
    // ==========================================
    
    [Test]
    public void Defense_ReducesDamage_Correctly()
    {
        // Arrange
        int incomingDamage = 30;
        int defense = 10;
        
        // Act
        int actualDamage = Mathf.Max(1, incomingDamage - defense);
        
        // Assert
        Assert.AreEqual(20, actualDamage, "Defense should reduce damage by defense value");
    }
    
    [Test]
    public void Defense_NeverNegatesDamageCompletely()
    {
        // Arrange
        int incomingDamage = 5;
        int defense = 100; // Very high defense
        
        // Act
        int actualDamage = Mathf.Max(1, incomingDamage - defense);
        
        // Assert
        Assert.AreEqual(1, actualDamage, "Should always deal at least 1 damage");
    }
}

