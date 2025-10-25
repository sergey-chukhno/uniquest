using UnityEngine;

/// <summary>
/// Static class to store battle information between scenes
/// This allows us to use ONE battle scene for all battles
/// </summary>
public static class BattleData
{
    // Which enemy to fight (1, 2, or 3)
    public static int enemyToFightIndex = 1;
    
    // Which background to use (1, 2, or 3)
    public static int battleBackgroundIndex = 1;
    
    // Battle zone name (for debugging)
    public static string battleZoneName = "Unknown";
    
    // Player's current stats (to carry into battle)
    public static int playerCurrentHealth = 100;
    public static int playerMaxHealth = 100;
    public static int playerCurrentMana = 50;
    public static int playerMaxMana = 50;
    
    // Player's inventory (to carry through battle)
    public static int healthPotionCount = 3;
    public static int manaPotionCount = 2;
    
    /// <summary>
    /// Set up battle data before transitioning to battle scene
    /// </summary>
    public static void SetupBattle(int enemy, int background, string zoneName)
    {
        enemyToFightIndex = enemy;
        battleBackgroundIndex = background;
        battleZoneName = zoneName;
        
        Debug.Log($"Battle setup: Enemy {enemy}, Background {background}, Zone: {zoneName}");
    }
    
    /// <summary>
    /// Update player stats (call this before battle)
    /// </summary>
    public static void UpdatePlayerStats(int health, int maxHealth, int mana, int maxMana)
    {
        playerCurrentHealth = health;
        playerMaxHealth = maxHealth;
        playerCurrentMana = mana;
        playerMaxMana = maxMana;
        
        Debug.Log($"BattleData updated: HP {health}/{maxHealth}, MP {mana}/{maxMana}");
    }
    
    /// <summary>
    /// Reset battle data to defaults
    /// </summary>
    public static void ResetBattleData()
    {
        enemyToFightIndex = 1;
        battleBackgroundIndex = 1;
        battleZoneName = "Unknown";
        playerCurrentHealth = 100;
        playerMaxHealth = 100;
        playerCurrentMana = 50;
        playerMaxMana = 50;
    }
}

