using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the player's inventory of items
/// </summary>
public class Inventory : MonoBehaviour
{
    [Header("Inventory Data")]
    public List<Item> items = new List<Item>();
    
    [Header("Starting Items")]
    public int startingHealthPotions = 3;
    public int startingManaPotions = 2;
    
    [Header("Events")]
    public System.Action<Item> OnItemAdded;
    public System.Action<Item> OnItemUsed;
    
    [Header("Initialization")]
    [System.NonSerialized] // Don't serialize, but will be set by code
    private bool hasInitialized = false;
    
    void Start()
    {
        Debug.Log($"Inventory.Start() - hasInitialized: {hasInitialized}, items.Count: {items.Count}");
        
        // Only initialize if inventory is completely empty
        // If items exist, they were set by SaveManager or UpdateStatsFromBattle
        if (items.Count == 0 && !hasInitialized)
        {
            // Give player some starting items
            AddItem(new Item("Health Potion", "Restores 30 HP", ItemType.HealthPotion, 30, startingHealthPotions));
            AddItem(new Item("Mana Potion", "Restores 25 MP", ItemType.ManaPotion, 25, startingManaPotions));
            
            hasInitialized = true;
            Debug.Log($"Inventory initialized with starting items: {items.Count} item types");
        }
        else if (items.Count > 0)
        {
            int hpCount = GetItemQuantity(ItemType.HealthPotion);
            int mpCount = GetItemQuantity(ItemType.ManaPotion);
            Debug.Log($"Inventory already has {items.Count} items (HP: {hpCount}, MP: {mpCount}) - skipping initialization");
            hasInitialized = true; // Mark as initialized so it doesn't add items later
        }
        else
        {
            Debug.Log($"Inventory already initialized: {items.Count} items, hasInitialized: {hasInitialized}");
        }
    }
    
    /// <summary>
    /// Add an item to inventory
    /// </summary>
    public void AddItem(Item newItem)
    {
        Debug.Log($"AddItem called: {newItem.name} ({newItem.type}) x{newItem.quantity}");
        
        // Check if we already have this item (match by type, not name, for consistency)
        Item existingItem = items.Find(item => item.type == newItem.type);
        
        if (existingItem != null)
        {
            // Add to existing quantity
            existingItem.quantity += newItem.quantity;
            Debug.Log($"Added {newItem.quantity} {newItem.name} to existing stack. Total: {existingItem.quantity}");
        }
        else
        {
            // Add new item
            items.Add(newItem);
            Debug.Log($"Added new item: {newItem.name} (x{newItem.quantity})");
        }
        
        Debug.Log($"Inventory now has {items.Count} item types:");
        foreach (Item item in items)
        {
            Debug.Log($"  - {item.name} ({item.type}): {item.quantity}");
        }
        
        // Notify UI that inventory changed
        OnItemAdded?.Invoke(newItem);
    }
    
    /// <summary>
    /// Use an item (consume it and apply its effect)
    /// </summary>
    public bool UseItem(Item item)
    {
        if (item.quantity <= 0)
        {
            Debug.Log($"Cannot use {item.name} - none remaining!");
            return false;
        }
        
        // Reduce quantity
        item.quantity--;
        Debug.Log($"Used {item.name}. Remaining: {item.quantity}");
        
        // Notify UI that item was used
        OnItemUsed?.Invoke(item);
        
        // Remove item if quantity reaches 0
        if (item.quantity <= 0)
        {
            items.Remove(item);
            Debug.Log($"Removed {item.name} from inventory (empty)");
        }
        
        return true;
    }
    
    /// <summary>
    /// Get an item by name
    /// </summary>
    public Item GetItem(string itemName)
    {
        return items.Find(item => item.name == itemName);
    }
    
    /// <summary>
    /// Get all items of a specific type
    /// </summary>
    public List<Item> GetItemsOfType(ItemType type)
    {
        return items.FindAll(item => item.type == type);
    }
    
    /// <summary>
    /// Check if player has any health potions
    /// </summary>
    public bool HasHealthPotion()
    {
        return GetItemsOfType(ItemType.HealthPotion).Count > 0;
    }
    
    /// <summary>
    /// Check if player has any mana potions
    /// </summary>
    public bool HasManaPotion()
    {
        return GetItemsOfType(ItemType.ManaPotion).Count > 0;
    }
    
    /// <summary>
    /// Use a health potion and return how much health it restored
    /// </summary>
    public int UseHealthPotion()
    {
        List<Item> healthPotions = GetItemsOfType(ItemType.HealthPotion);
        if (healthPotions.Count > 0)
        {
            Item potion = healthPotions[0]; // Use first health potion
            if (UseItem(potion))
            {
                return potion.value;
            }
        }
        return 0; // No health restored
    }
    
    /// <summary>
    /// Use a mana potion and return how much mana it restored
    /// </summary>
    public int UseManaPotion()
    {
        List<Item> manaPotions = GetItemsOfType(ItemType.ManaPotion);
        if (manaPotions.Count > 0)
        {
            Item potion = manaPotions[0]; // Use first mana potion
            if (UseItem(potion))
            {
                return potion.value;
            }
        }
        return 0; // No mana restored
    }
    
    /// <summary>
    /// Get total number of items in inventory
    /// </summary>
    public int GetTotalItemCount()
    {
        int total = 0;
        foreach (Item item in items)
        {
            total += item.quantity;
        }
        return total;
    }
    
    /// <summary>
    /// Get the quantity of a specific item type
    /// </summary>
    public int GetItemQuantity(ItemType itemType)
    {
        Item item = items.Find(i => i.type == itemType);
        return item != null ? item.quantity : 0;
    }
    
    /// <summary>
    /// Mark inventory as initialized (used when loading from save)
    /// </summary>
    public void MarkAsInitialized()
    {
        hasInitialized = true;
        Debug.Log("Inventory marked as initialized (loaded from save)");
    }
}
