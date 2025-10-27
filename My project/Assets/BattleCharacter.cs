using UnityEngine;

public class BattleCharacter : MonoBehaviour
{
    [Header("Character Stats")]
    public string characterName;
    public int maxHealth = 100;
    public int currentHealth;
    public int attackPower = 20;
    public int defense = 10;
    public int maxMana = 50;
    public int currentMana;
    
    [Header("Character Visual")]
    public Sprite characterSprite;
    
    [Header("Battle UI")]
    public UnityEngine.UI.Slider healthBar;
    public UnityEngine.UI.Slider manaBar;
    
    [Header("Battle State")]
    public bool isPlayer = false;
    public bool isAlive = true;
    
    [Header("Visual Effects")]
    public DamageFlash damageFlash;
    public AttackAnimation attackAnimation;
    
    void Start()
    {
        // DON'T initialize health and mana here - BattleManager will set them
        // from BattleData in LoadPlayerData() and LoadEnemyData()
        
        // Just initialize the UI bars to be ready
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
        }
        
        if (manaBar != null)
        {
            manaBar.maxValue = maxMana;
        }
        
        Debug.Log($"{characterName} waiting for initialization from BattleManager...");
    }
    
    /// <summary>
    /// Initialize character with specific values (called by BattleManager)
    /// </summary>
    public void InitializeStats()
    {
        // Update UI bars with current values
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
        
        if (manaBar != null)
        {
            manaBar.maxValue = maxMana;
            manaBar.value = currentMana;
        }
        
        Debug.Log($"{characterName} initialized - HP: {currentHealth}/{maxHealth}, MP: {currentMana}/{maxMana}");
    }
    
    // Take damage from enemy attack
    public void TakeDamage(int damage)
    {
        // Calculate actual damage (reduce by defense)
        int actualDamage = Mathf.Max(1, damage - defense);
        currentHealth -= actualDamage;
        
        // Ensure health doesn't go below 0
        currentHealth = Mathf.Max(0, currentHealth);
        
        // Update health bar
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }
        
        // Trigger damage flash effect
        if (damageFlash != null)
        {
            damageFlash.Flash();
        }
        
        Debug.Log($"{characterName} takes {actualDamage} damage! HP: {currentHealth}/{maxHealth}");
        
        // Check if character is dead
        if (currentHealth <= 0)
        {
            isAlive = false;
            Debug.Log($"{characterName} is defeated!");
        }
    }
    
    // Basic attack
    public int BasicAttack()
    {
        // Trigger attack animation
        if (attackAnimation != null)
        {
            attackAnimation.PlayAttackAnimation(isPlayer);
        }
        
        Debug.Log($"{characterName} uses Basic Attack!");
        return attackPower;
    }
    
    // Super attack (costs mana)
    public int SuperAttack()
    {
        if (currentMana >= 20) // Super attack costs 20 mana
        {
            currentMana -= 20;
            
            // Update mana bar if assigned
            if (manaBar != null)
            {
                manaBar.value = currentMana;
            }
            
            // Trigger super attack animation
            if (attackAnimation != null)
            {
                attackAnimation.PlaySuperAttackAnimation(isPlayer);
            }
            
            int superAttackPower = attackPower * 2; // Double damage
            Debug.Log($"{characterName} uses Super Attack! Damage: {superAttackPower}");
            return superAttackPower;
        }
        else
        {
            Debug.Log($"{characterName} doesn't have enough mana for Super Attack!");
            return 0; // No damage if not enough mana
        }
    }
    
    // Heal (player only, costs mana)
    public void Heal(int healAmount)
    {
        if (isPlayer && currentMana >= 15) // Heal costs 15 mana
        {
            currentMana -= 15;
            currentHealth = Mathf.Min(maxHealth, currentHealth + healAmount);
            
            // Update UI
            if (healthBar != null)
            {
                healthBar.value = currentHealth;
            }
            if (manaBar != null)
            {
                manaBar.value = currentMana;
            }
            
            Debug.Log($"{characterName} heals for {healAmount}! HP: {currentHealth}/{maxHealth}");
        }
    }
    
    // Restore some mana each turn
    public void RestoreMana(int amount)
    {
        currentMana = Mathf.Min(maxMana, currentMana + amount);
        
        if (manaBar != null)
        {
            manaBar.value = currentMana;
        }
    }
    
    // Check if character can perform super attack
    public bool CanSuperAttack()
    {
        return currentMana >= 20;
    }
    
    // Get current health percentage (0-1)
    public float GetHealthPercentage()
    {
        return (float)currentHealth / maxHealth;
    }
    
    // Get current mana percentage (0-1)
    public float GetManaPercentage()
    {
        return (float)currentMana / maxMana;
    }
    
    // Update character sprite
    public void UpdateSprite(Sprite newSprite)
    {
        characterSprite = newSprite;
        
        // Update the SpriteRenderer component
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = newSprite;
            Debug.Log($"{characterName} sprite updated!");
        }
        else
        {
            Debug.LogWarning($"No SpriteRenderer found on {characterName}!");
        }
    }
}
