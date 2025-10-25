using UnityEngine;
using TMPro;

/// <summary>
/// Displays the player's item inventory counts on screen
/// </summary>
public class ItemCounter : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI healthPotionText;
    public TextMeshProUGUI manaPotionText;
    
    [Header("Inventory Reference")]
    public Inventory playerInventory;
    
    [Header("Display Format")]
    public string healthPotionLabel = "HP Potion: ";
    public string manaPotionLabel = "MP Potion: ";
    
    void Start()
    {
        // Find player inventory if not assigned
        if (playerInventory == null)
        {
            // Try to find by tag first
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerInventory = player.GetComponent<Inventory>();
                Debug.Log($"ItemCounter: Found Player with tag, Inventory component: {playerInventory != null}");
            }
            else
            {
                Debug.LogWarning("ItemCounter: Could not find GameObject with 'Player' tag!");
            }
            
            // If still null, try to find any Inventory in scene
            if (playerInventory == null)
            {
                playerInventory = FindObjectOfType<Inventory>();
                if (playerInventory != null)
                {
                    Debug.Log("ItemCounter: Found Inventory using FindObjectOfType");
                }
            }
        }
        
        // Verify references
        if (healthPotionText == null)
        {
            Debug.LogError("ItemCounter: Health Potion Text is not assigned!");
        }
        
        if (manaPotionText == null)
        {
            Debug.LogError("ItemCounter: Mana Potion Text is not assigned!");
        }
        
        if (playerInventory == null)
        {
            Debug.LogError("ItemCounter: Player Inventory not found! Make sure Player has Inventory component and 'Player' tag!");
        }
        else
        {
            Debug.Log("ItemCounter: Successfully initialized with Inventory!");
        }
        
        // Initial update
        UpdateItemCounters();
    }
    
    void Update()
    {
        // Update counters every frame (lightweight operation)
        UpdateItemCounters();
    }
    
    /// <summary>
    /// Updates the item counter display with current inventory quantities
    /// </summary>
    void UpdateItemCounters()
    {
        if (playerInventory == null) return;
        
        // Get current quantities
        int healthPotionCount = playerInventory.GetItemQuantity(ItemType.HealthPotion);
        int manaPotionCount = playerInventory.GetItemQuantity(ItemType.ManaPotion);
        
        // Update health potion text
        if (healthPotionText != null)
        {
            healthPotionText.text = $"{healthPotionLabel}{healthPotionCount}";
            
            // Optional: Change color based on quantity
            if (healthPotionCount == 0)
            {
                healthPotionText.color = new Color(1f, 1f, 1f, 0.5f); // Faded white when empty
            }
            else
            {
                healthPotionText.color = new Color(0.5f, 1f, 0.5f, 1f); // Light green when available
            }
        }
        
        // Update mana potion text
        if (manaPotionText != null)
        {
            manaPotionText.text = $"{manaPotionLabel}{manaPotionCount}";
            
            // Optional: Change color based on quantity
            if (manaPotionCount == 0)
            {
                manaPotionText.color = new Color(1f, 1f, 1f, 0.5f); // Faded white when empty
            }
            else
            {
                manaPotionText.color = new Color(0.5f, 0.8f, 1f, 1f); // Light blue when available
            }
        }
    }
    
    /// <summary>
    /// Manually trigger an update (useful for debugging or specific events)
    /// </summary>
    public void ForceUpdate()
    {
        UpdateItemCounters();
    }
}

