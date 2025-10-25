using UnityEngine;

/// <summary>
/// Represents an item that can be collected and used
/// </summary>
[System.Serializable]
public class Item
{
    public string name;
    public string description;
    public ItemType type;
    public int quantity;
    public int value; // How much health/mana it restores
    
    public Item(string itemName, string desc, ItemType itemType, int itemValue, int qty = 1)
    {
        name = itemName;
        description = desc;
        type = itemType;
        value = itemValue;
        quantity = qty;
    }
}

/// <summary>
/// Types of items in the game
/// </summary>
public enum ItemType
{
    HealthPotion,
    ManaPotion,
    // Can add more types later if needed
}
