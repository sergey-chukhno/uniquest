using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    
    [Header("Animation")]
    public Animator animator;
    
    [Header("Directional Movement")]
    public SpriteRenderer spriteRenderer;
    
    [Header("Collision")]
    public BoxCollider2D playerCollider;
    
    [Header("Bush Hiding")]
    public float bushAlpha = 0.5f; // How transparent player becomes in bushes
    private float originalAlpha = 1f;
    private bool isInBush = false;
    
    [Header("Battle System")]
    private bool hasTriggeredBattle = false; // Prevent multiple battle triggers
    private string lastInteractedZone = ""; // Track last zone to prevent spam
    
    [Header("Inventory System")]
    public Inventory inventory;
    
    [Header("Collection Tracking")]
    private System.Collections.Generic.HashSet<string> collectedObjects = new System.Collections.Generic.HashSet<string>();
    
    [Header("Player Stats")]
    public int maxHealth = 100;
    public int currentHealth = 100;
    public int maxMana = 50;
    public int currentMana = 50;
    private bool statsInitialized = false;
    
    [Header("UI Messages")]
    public MessageDisplay messageDisplay;
    
    void Start()
    {
        // Store original alpha value
        originalAlpha = spriteRenderer.color.a;
        
        // Initialize inventory if not assigned
        if (inventory == null)
        {
            inventory = GetComponent<Inventory>();
            if (inventory == null)
            {
                inventory = gameObject.AddComponent<Inventory>();
            }
        }
        
        // Only initialize stats if not already set (e.g., from battle or save)
        if (!statsInitialized)
        {
            currentHealth = maxHealth;
            currentMana = maxMana;
            statsInitialized = true;
            Debug.Log($"Player initialized with default stats: HP {currentHealth}/{maxHealth}, MP {currentMana}/{maxMana}");
        }
        else
        {
            Debug.Log($"Player stats already initialized: HP {currentHealth}/{maxHealth}, MP {currentMana}/{maxMana}");
        }
        
        Debug.Log("Player initialized with inventory system");
        
        // Initialize message display if not assigned
        if (messageDisplay == null)
        {
            messageDisplay = FindObjectOfType<MessageDisplay>();
        }
    }
    
    void ShowMessage(string message)
    {
        // Show message on screen if available
        if (messageDisplay != null)
        {
            messageDisplay.ShowMessage(message);
        }
        
        // Also log to console
        Debug.Log(message);
    }
    
    void Update()
    {
        // Get input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        // Create movement vector
        Vector2 movement = new Vector2(horizontal, vertical);
        
        // Check if player is moving
        bool isMoving = Mathf.Abs(horizontal) > 0.3f || Mathf.Abs(vertical) > 0.3f;
        
        // Handle directional movement with simple flipping
        if (isMoving)
        {
            if (horizontal > 0.1f) // Moving right
            {
                spriteRenderer.flipX = false; // Face right
            }
            else if (horizontal < -0.1f) // Moving left
            {
                spriteRenderer.flipX = true; // Face left
            }
            // For up/down movement, we keep the current horizontal facing
        }
        
        // Set animation parameter
        if (animator != null)
        {
            animator.SetBool("IsMoving", isMoving);
        }
        
        // Handle item usage
        HandleItemUsage();
        
        // Move player with collision detection
        MoveWithCollision(movement);
        
        // Check for bush hiding
        CheckBushHiding();
        
        // Check for interactions
        CheckInteractions();
    }
    
    void MoveWithCollision(Vector2 movement)
    {
        // Calculate new position
        Vector2 newPosition = (Vector2)transform.position + movement * moveSpeed * Time.deltaTime;
        
        // Check for collisions before moving
        if (CanMoveTo(newPosition))
        {
            transform.Translate(movement * moveSpeed * Time.deltaTime);
        }
    }
    
    bool CanMoveTo(Vector2 targetPosition)
    {
        // Check if there are any collisions at the target position
        Collider2D[] colliders = Physics2D.OverlapBoxAll(
            targetPosition,
            playerCollider.bounds.size,
            0f
        );
        
        // Check if any colliders are solid (not triggers)
        foreach (Collider2D col in colliders)
        {
            if (col != playerCollider && !col.isTrigger)
            {
                return false; // Can't move here
            }
        }
        
        return true; // Can move here
    }
    
    void CheckBushHiding()
    {
        // Check if player is overlapping with any bush
        Collider2D[] colliders = Physics2D.OverlapBoxAll(
            playerCollider.bounds.center,
            playerCollider.bounds.size,
            0f
        );
        
        bool foundBush = false;
        foreach (Collider2D col in colliders)
        {
            if (col != playerCollider && col.CompareTag("Bush"))
            {
                foundBush = true;
                break;
            }
        }
        
        // Update bush hiding state
        if (foundBush && !isInBush)
        {
            // Entering bush - make player semi-transparent
            isInBush = true;
            Color color = spriteRenderer.color;
            color.a = bushAlpha;
            spriteRenderer.color = color;
        }
        else if (!foundBush && isInBush)
        {
            // Exiting bush - make player fully visible
            isInBush = false;
            Color color = spriteRenderer.color;
            color.a = originalAlpha;
            spriteRenderer.color = color;
        }
    }
    
    void CheckInteractions()
    {
        // Check if player is overlapping with any interactive object
        Collider2D[] colliders = Physics2D.OverlapBoxAll(
            playerCollider.bounds.center,
            playerCollider.bounds.size,
            0f
        );
        
        bool currentlyInBattleZone = false;
        string currentBattleZone = "";
        
        foreach (Collider2D col in colliders)
        {
            if (col != playerCollider)
            {
                if (col.CompareTag("BattleZone"))
                {
                    currentlyInBattleZone = true;
                    currentBattleZone = col.gameObject.name;
                }
                else if (col.CompareTag("Chest"))
                {
                    // Get mana potion from chest
                    CollectItem(col.gameObject, "Chest", ItemType.ManaPotion, 25, 1);
                }
                else if (col.CompareTag("Well"))
                {
                    // Get health potion from well
                    CollectItem(col.gameObject, "Well", ItemType.HealthPotion, 30, 1);
                }
                else if (col.CompareTag("VictoryFlag"))
                {
                    // Check if all trolls are defeated
                    CheckVictoryFlag();
                }
            }
        }
        
        // Handle battle zone interactions after checking all colliders
        if (currentlyInBattleZone)
        {
            // Check if we entered a new battle zone or if we haven't triggered battle yet
            if (currentBattleZone != lastInteractedZone && !hasTriggeredBattle)
            {
                // Find the battle zone GameObject
                GameObject battleZoneObject = GameObject.Find(currentBattleZone);
                if (battleZoneObject != null)
                {
                    CheckBattleZone(battleZoneObject);
                }
                lastInteractedZone = currentBattleZone;
            }
        }
        else
        {
            // Player left all battle zones - reset flags
            if (lastInteractedZone != "")
            {
                lastInteractedZone = "";
                // Don't reset hasTriggeredBattle here - it should only reset when returning from battle
            }
        }
    }
    
    void CollectItem(GameObject collectionObject, string objectType, ItemType itemType, int itemValue, int quantity)
    {
        // Create unique ID for this collection object
        string objectId = collectionObject.name + "_" + collectionObject.GetInstanceID();
        
        // Check if we've already collected from this object
        if (collectedObjects.Contains(objectId))
        {
            Debug.Log($"{objectType} already collected from {collectionObject.name}");
            return;
        }
        
        // Create the item to add
        Item collectedItem = new Item(
            itemType == ItemType.HealthPotion ? "Health Potion" : "Mana Potion",
            itemType == ItemType.HealthPotion ? "Restores 30 HP" : "Restores 25 MP",
            itemType,
            itemValue,
            quantity
        );
        
        // Add item to inventory
        if (inventory != null)
        {
            inventory.AddItem(collectedItem);
            
            // Mark this object as collected
            collectedObjects.Add(objectId);
            
            // Provide feedback to player
            string itemName = itemType == ItemType.HealthPotion ? "Health Potion" : "Mana Potion";
            ShowMessage($"[FOUND] {itemName} in {objectType}! Added to inventory.");
            
            // Optional: Hide or change the object appearance
            // collectionObject.SetActive(false); // Uncomment to hide object after collection
        }
        else
        {
            Debug.LogError("Inventory is null! Cannot collect item.");
        }
    }
    
    void HandleItemUsage()
    {
        // Use health potion with H key
        if (Input.GetKeyDown(KeyCode.H))
        {
            UseHealthPotion();
        }
        
        // Use mana potion with N key
        if (Input.GetKeyDown(KeyCode.N))
        {
            UseManaPotion();
        }
    }
    
    void UseHealthPotion()
    {
        if (inventory != null && inventory.HasHealthPotion())
        {
            int restoredHealth = inventory.UseHealthPotion();
            currentHealth = Mathf.Min(maxHealth, currentHealth + restoredHealth);
            ShowMessage($"[HEALTH POTION] Restored {restoredHealth} HP! Current HP: {currentHealth}/{maxHealth}");
        }
        else
        {
            ShowMessage("[NO POTION] No health potions available!");
        }
    }
    
    void UseManaPotion()
    {
        if (inventory != null && inventory.HasManaPotion())
        {
            int oldMana = currentMana;
            int restoredMana = inventory.UseManaPotion();
            currentMana = Mathf.Min(maxMana, currentMana + restoredMana);
            Debug.Log($"UseManaPotion: Mana changed from {oldMana} to {currentMana} (restored {restoredMana})");
            ShowMessage($"[MANA POTION] Restored {restoredMana} MP! Current MP: {currentMana}/{maxMana}");
        }
        else
        {
            Debug.Log($"UseManaPotion: No mana potions available! Current MP: {currentMana}");
            ShowMessage("[NO POTION] No mana potions available!");
        }
    }
    
    void CheckVictoryFlag()
    {
        if (GameProgress.IsGameCompleted())
        {
            // All trolls defeated - show victory message
            ShowMessage("[VICTORY] You have defeated all three trolls!");
            ShowMessage("[VICTORY] You saved the land! Congratulations!");
            ShowMessage("[VICTORY] Adventure complete!");
            // TODO: Load victory scene or show victory UI
        }
        else
        {
            // Not all trolls defeated yet
            int defeatedCount = GameProgress.GetDefeatedTrollCount();
            int remainingCount = 3 - defeatedCount;
            ShowMessage($"[PROGRESS] {defeatedCount}/3 trolls defeated. {remainingCount} more to go!");
        }
    }
    
    void CheckBattleZone(GameObject battleZone)
    {
        // Determine which troll this battle zone contains
        string zoneName = battleZone.name;
        int trollIndex = GetTrollIndexFromZone(zoneName);
        
        Debug.Log($"CheckBattleZone: zoneName='{zoneName}', trollIndex={trollIndex}");
        Debug.Log($"CheckBattleZone: GameProgress.IsTrollDefeated({trollIndex}) = {GameProgress.IsTrollDefeated(trollIndex)}");
        
        // Check if this troll has already been defeated
        if (GameProgress.IsTrollDefeated(trollIndex))
        {
            Debug.Log($"Troll {trollIndex} already defeated - showing victory message");
            // Troll already defeated - show victory message (only once per zone entry)
            if (zoneName != lastInteractedZone)
            {
                ShowMessage(GameProgress.GetVictoryMessage(trollIndex));
            }
            return;
        }
        
        Debug.Log($"Troll {trollIndex} not defeated - starting battle");
        // Troll not defeated - start battle
        TriggerBattle(battleZone);
    }
    
    int GetTrollIndexFromZone(string zoneName)
    {
        Debug.Log($"GetTrollIndexFromZone called with zoneName: '{zoneName}'");
        
        // Map battle zones to troll indices
        if (zoneName.Contains("Tent") || zoneName.Contains("tent"))
        {
            Debug.Log("Zone contains 'Tent' - returning Troll 1");
            return 1; // Troll 1
        }
        else if (zoneName.Contains("Round") || zoneName.Contains("round"))
        {
            Debug.Log("Zone contains 'Round' - returning Troll 2");
            return 2; // Troll 2
        }
        else if (zoneName.Contains("Square") || zoneName.Contains("square"))
        {
            Debug.Log("Zone contains 'Square' - returning Troll 3");
            return 3; // Troll 3
        }
        else
        {
            Debug.Log($"Zone '{zoneName}' doesn't match any pattern - defaulting to Troll 1");
            return 1; // Default to Troll 1
        }
    }
    
    void TriggerBattle(GameObject battleZone)
    {
        hasTriggeredBattle = true;
        
        // Determine which battle zone this is based on name
        string zoneName = battleZone.name;
        int enemyIndex = 1;
        int backgroundIndex = 1;
        
        // Map battle zones to enemies and backgrounds
        if (zoneName.Contains("Tent") || zoneName.Contains("tent"))
        {
            enemyIndex = 1; // Troll 1
            backgroundIndex = 1; // Battleground 1
            Debug.Log("Entering Tent - Fighting Troll 1");
        }
        else if (zoneName.Contains("Round") || zoneName.Contains("round"))
        {
            enemyIndex = 2; // Troll 2
            backgroundIndex = 2; // Battleground 2
            Debug.Log("Entering Castle Round - Fighting Troll 2");
        }
        else if (zoneName.Contains("Square") || zoneName.Contains("square"))
        {
            enemyIndex = 3; // Troll 3
            backgroundIndex = 3; // Battleground 3
            Debug.Log("Entering Castle Square - Fighting Troll 3");
        }
        else
        {
            // Default battle zone
            Debug.Log($"Entering {zoneName} - Fighting Troll 1 (default)");
        }
        
        // Set up battle data
        Debug.Log($"Setting up battle: enemyIndex={enemyIndex}, backgroundIndex={backgroundIndex}, zoneName='{zoneName}'");
        BattleData.SetupBattle(enemyIndex, backgroundIndex, zoneName);
        
        // Update player stats before battle
        Debug.Log($"BEFORE UpdatePlayerStats - currentMana = {currentMana}, maxMana = {maxMana}");
        BattleData.UpdatePlayerStats(currentHealth, maxHealth, currentMana, maxMana);
        
        // Save inventory counts to BattleData
        if (inventory != null)
        {
            int hpCount = inventory.GetItemQuantity(ItemType.HealthPotion);
            int mpCount = inventory.GetItemQuantity(ItemType.ManaPotion);
            
            Debug.Log($"BEFORE saving - Inventory has: HP potions: {hpCount}, MP potions: {mpCount}");
            Debug.Log($"BEFORE saving - BattleData has: HP potions: {BattleData.healthPotionCount}, MP potions: {BattleData.manaPotionCount}");
            
            BattleData.healthPotionCount = hpCount;
            BattleData.manaPotionCount = mpCount;
            
            Debug.Log($"AFTER saving to BattleData: HP potions: {BattleData.healthPotionCount}, MP potions: {BattleData.manaPotionCount}");
        }
        
        Debug.Log($"Triggering battle with player stats: HP {currentHealth}/{maxHealth}, MP {currentMana}/{maxMana}");
        
        // Load battle scene
        SceneManager.LoadScene("BattleScene");
    }
    
    // Called when returning from battle
    void OnEnable()
    {
        Debug.Log($"PlayerController.OnEnable() called - Inventory items count: {(inventory != null ? inventory.items.Count : -1)}");
        
        // Reset battle trigger when returning to map
        hasTriggeredBattle = false;
        
        // Update player stats from battle result
        UpdateStatsFromBattle();
        
        Debug.Log($"After UpdateStatsFromBattle - Inventory items count: {(inventory != null ? inventory.items.Count : -1)}");
        
        // Trigger auto-save if player just won a battle
        // (SaveManager will handle the actual saving)
        SaveManager saveManager = FindObjectOfType<SaveManager>();
        if (saveManager != null && GameProgress.GetDefeatedTrollCount() > 0)
        {
            // Small delay to ensure all data is updated
            Invoke(nameof(TriggerAutoSave), 0.5f);
        }
    }
    
    void TriggerAutoSave()
    {
        SaveManager saveManager = FindObjectOfType<SaveManager>();
        if (saveManager != null)
        {
            saveManager.AutoSave();
        }
    }
    
    void UpdateStatsFromBattle()
    {
        Debug.Log($"UpdateStatsFromBattle called - BattleData has: HP {BattleData.playerCurrentHealth}/{BattleData.playerMaxHealth}, MP {BattleData.playerCurrentMana}/{BattleData.playerMaxMana}");
        
        // Update current health and mana from BattleData
        currentHealth = BattleData.playerCurrentHealth;
        currentMana = BattleData.playerCurrentMana;
        maxHealth = BattleData.playerMaxHealth;
        maxMana = BattleData.playerMaxMana;
        statsInitialized = true; // Mark as initialized to prevent reset in Start()
        
        // Restore inventory from BattleData (delayed to ensure Inventory.Start() runs first)
        Invoke(nameof(RestoreInventoryFromBattle), 0.2f);
        
        Debug.Log($"Player stats updated to: HP {currentHealth}/{maxHealth}, MP {currentMana}/{maxMana}");
        
        // Check if this is a fresh game start (only reset inventory after game over)
        // Game over sets BOTH health and mana to MAX via ResetBattleData()
        bool isFreshStart = (currentHealth == 100 && maxHealth == 100 && 
                             currentMana == 50 && maxMana == 50 &&
                             currentHealth == maxHealth && currentMana == maxMana);
        
        if (isFreshStart)
        {
            Debug.Log("🌟 Starting new adventure! Full health and mana restored!");
            
            // Reset inventory to starting items (with delay to ensure Inventory.Start() runs first)
            if (inventory != null)
            {
                Invoke(nameof(ResetInventoryToDefault), 0.1f);
            }
        }
        else
        {
            Debug.Log($"Returned from battle - HP: {currentHealth}/{maxHealth}, MP: {currentMana}/{maxMana}");
        }
    }
    
    /// <summary>
    /// Restore inventory from BattleData (after returning from battle)
    /// </summary>
    void RestoreInventoryFromBattle()
    {
        if (inventory != null)
        {
            Debug.Log($"RestoreInventoryFromBattle called - BattleData has HP: {BattleData.healthPotionCount}, MP: {BattleData.manaPotionCount}");
            
            inventory.items.Clear();
            
            if (BattleData.healthPotionCount > 0)
            {
                inventory.AddItem(new Item("Health Potion", "Restores 30 HP", ItemType.HealthPotion, 30, BattleData.healthPotionCount));
            }
            
            if (BattleData.manaPotionCount > 0)
            {
                inventory.AddItem(new Item("Mana Potion", "Restores 25 MP", ItemType.ManaPotion, 25, BattleData.manaPotionCount));
            }
            
            inventory.MarkAsInitialized();
            Debug.Log($"Inventory restored from BattleData: HP potions: {BattleData.healthPotionCount}, MP potions: {BattleData.manaPotionCount}, items.Count: {inventory.items.Count}");
        }
    }
    
    /// <summary>
    /// Reset inventory to default starting items (for game over/fresh start)
    /// </summary>
    void ResetInventoryToDefault()
    {
        if (inventory != null)
        {
            inventory.items.Clear();
            inventory.AddItem(new Item("Health Potion", "Restores 30 HP", ItemType.HealthPotion, 30, 3));
            inventory.AddItem(new Item("Mana Potion", "Restores 25 MP", ItemType.ManaPotion, 25, 2));
            inventory.MarkAsInitialized();
            Debug.Log("Inventory reset to default starting items");
        }
    }
    
    // ===== SAVE/LOAD SUPPORT METHODS =====
    
    /// <summary>
    /// Get the set of collected object IDs (for saving)
    /// </summary>
    public System.Collections.Generic.HashSet<string> GetCollectedObjects()
    {
        return collectedObjects;
    }
    
    /// <summary>
    /// Set the collected objects (for loading)
    /// </summary>
    public void SetCollectedObjects(System.Collections.Generic.List<string> objects)
    {
        collectedObjects.Clear();
        foreach (string obj in objects)
        {
            collectedObjects.Add(obj);
        }
        Debug.Log($"Loaded {collectedObjects.Count} collected objects from save");
    }
}
