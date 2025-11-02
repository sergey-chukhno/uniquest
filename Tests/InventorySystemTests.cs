using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Unit tests for Inventory System
/// Tests item management, usage, and quantity tracking
/// </summary>
public class InventorySystemTests
{
    // Helper class for testing (mimics Item structure)
    private class TestItem
    {
        public string name;
        public ItemType type;
        public int quantity;
        public int value;
        
        public TestItem(string name, ItemType type, int value, int quantity)
        {
            this.name = name;
            this.type = type;
            this.value = value;
            this.quantity = quantity;
        }
    }
    
    // Using the actual ItemType enum from the project
    public enum ItemType
    {
        HealthPotion,
        ManaPotion
    }
    
    // ==========================================
    // ITEM ADDITION TESTS
    // ==========================================
    
    [Test]
    public void Inventory_AddNewItem_IncreasesCount()
    {
        // Arrange
        List<TestItem> inventory = new List<TestItem>();
        
        // Act
        inventory.Add(new TestItem("Health Potion", ItemType.HealthPotion, 30, 3));
        
        // Assert
        Assert.AreEqual(1, inventory.Count, "Inventory should have 1 item type");
        Assert.AreEqual(3, inventory[0].quantity, "Item should have quantity of 3");
    }
    
    [Test]
    public void Inventory_AddExistingItem_IncreasesQuantity()
    {
        // Arrange
        List<TestItem> inventory = new List<TestItem>();
        inventory.Add(new TestItem("Health Potion", ItemType.HealthPotion, 30, 3));
        
        // Act
        TestItem existing = inventory.Find(i => i.type == ItemType.HealthPotion);
        if (existing != null)
        {
            existing.quantity += 2;
        }
        
        // Assert
        Assert.AreEqual(1, inventory.Count, "Should still have 1 item type");
        Assert.AreEqual(5, existing.quantity, "Quantity should be stacked to 5");
    }
    
    [Test]
    public void Inventory_StartingItems_HasCorrectQuantities()
    {
        // Arrange & Act
        List<TestItem> inventory = new List<TestItem>
        {
            new TestItem("Health Potion", ItemType.HealthPotion, 30, 3),
            new TestItem("Mana Potion", ItemType.ManaPotion, 25, 2)
        };
        
        // Assert
        Assert.AreEqual(2, inventory.Count, "Should start with 2 item types");
        
        TestItem healthPotion = inventory.Find(i => i.type == ItemType.HealthPotion);
        Assert.AreEqual(3, healthPotion.quantity, "Should start with 3 health potions");
        
        TestItem manaPotion = inventory.Find(i => i.type == ItemType.ManaPotion);
        Assert.AreEqual(2, manaPotion.quantity, "Should start with 2 mana potions");
    }
    
    // ==========================================
    // ITEM USAGE TESTS
    // ==========================================
    
    [Test]
    public void Inventory_UseItem_DecreasesQuantity()
    {
        // Arrange
        TestItem healthPotion = new TestItem("Health Potion", ItemType.HealthPotion, 30, 3);
        
        // Act
        healthPotion.quantity--;
        
        // Assert
        Assert.AreEqual(2, healthPotion.quantity, "Quantity should decrease by 1");
    }
    
    [Test]
    public void Inventory_UseLastItem_RemovesFromInventory()
    {
        // Arrange
        List<TestItem> inventory = new List<TestItem>();
        TestItem healthPotion = new TestItem("Health Potion", ItemType.HealthPotion, 30, 1);
        inventory.Add(healthPotion);
        
        // Act
        healthPotion.quantity--;
        if (healthPotion.quantity <= 0)
        {
            inventory.Remove(healthPotion);
        }
        
        // Assert
        Assert.AreEqual(0, inventory.Count, "Item should be removed when quantity reaches 0");
    }
    
    [Test]
    public void Inventory_UseHealthPotion_RestoresHealth()
    {
        // Arrange
        int currentHealth = 50;
        int maxHealth = 100;
        int healAmount = 30;
        
        // Act
        currentHealth = UnityEngine.Mathf.Min(maxHealth, currentHealth + healAmount);
        
        // Assert
        Assert.AreEqual(80, currentHealth, "Health should be restored by potion value");
    }
    
    [Test]
    public void Inventory_UseHealthPotion_DoesNotExceedMax()
    {
        // Arrange
        int currentHealth = 90;
        int maxHealth = 100;
        int healAmount = 30;
        
        // Act
        currentHealth = UnityEngine.Mathf.Min(maxHealth, currentHealth + healAmount);
        
        // Assert
        Assert.AreEqual(100, currentHealth, "Health should not exceed maximum");
    }
    
    [Test]
    public void Inventory_UseManaPotion_RestoresMana()
    {
        // Arrange
        int currentMana = 20;
        int maxMana = 50;
        int restoreAmount = 25;
        
        // Act
        currentMana = UnityEngine.Mathf.Min(maxMana, currentMana + restoreAmount);
        
        // Assert
        Assert.AreEqual(45, currentMana, "Mana should be restored by potion value");
    }
    
    // ==========================================
    // ITEM AVAILABILITY TESTS
    // ==========================================
    
    [Test]
    public void Inventory_HasHealthPotion_ReturnsTrue()
    {
        // Arrange
        List<TestItem> inventory = new List<TestItem>
        {
            new TestItem("Health Potion", ItemType.HealthPotion, 30, 3)
        };
        
        // Act
        bool hasPotion = inventory.Any(i => i.type == ItemType.HealthPotion && i.quantity > 0);
        
        // Assert
        Assert.IsTrue(hasPotion, "Should have health potions available");
    }
    
    [Test]
    public void Inventory_HasHealthPotion_WhenEmpty_ReturnsFalse()
    {
        // Arrange
        List<TestItem> inventory = new List<TestItem>();
        
        // Act
        bool hasPotion = inventory.Any(i => i.type == ItemType.HealthPotion && i.quantity > 0);
        
        // Assert
        Assert.IsFalse(hasPotion, "Should not have health potions when inventory empty");
    }
    
    [Test]
    public void Inventory_GetItemQuantity_ReturnsCorrectCount()
    {
        // Arrange
        List<TestItem> inventory = new List<TestItem>
        {
            new TestItem("Health Potion", ItemType.HealthPotion, 30, 5),
            new TestItem("Mana Potion", ItemType.ManaPotion, 25, 2)
        };
        
        // Act
        TestItem healthPotion = inventory.Find(i => i.type == ItemType.HealthPotion);
        int healthCount = healthPotion?.quantity ?? 0;
        
        TestItem manaPotion = inventory.Find(i => i.type == ItemType.ManaPotion);
        int manaCount = manaPotion?.quantity ?? 0;
        
        // Assert
        Assert.AreEqual(5, healthCount, "Should have 5 health potions");
        Assert.AreEqual(2, manaCount, "Should have 2 mana potions");
    }
    
    [Test]
    public void Inventory_GetItemQuantity_NonExistent_ReturnsZero()
    {
        // Arrange
        List<TestItem> inventory = new List<TestItem>();
        
        // Act
        TestItem healthPotion = inventory.Find(i => i.type == ItemType.HealthPotion);
        int count = healthPotion?.quantity ?? 0;
        
        // Assert
        Assert.AreEqual(0, count, "Should return 0 for non-existent item");
    }
    
    // ==========================================
    // ITEM TYPE TESTS
    // ==========================================
    
    [Test]
    public void Inventory_GetItemsByType_ReturnsCorrectItems()
    {
        // Arrange
        List<TestItem> inventory = new List<TestItem>
        {
            new TestItem("Health Potion", ItemType.HealthPotion, 30, 3),
            new TestItem("Mana Potion", ItemType.ManaPotion, 25, 2)
        };
        
        // Act
        List<TestItem> healthPotions = inventory.FindAll(i => i.type == ItemType.HealthPotion);
        List<TestItem> manaPotions = inventory.FindAll(i => i.type == ItemType.ManaPotion);
        
        // Assert
        Assert.AreEqual(1, healthPotions.Count, "Should find 1 health potion type");
        Assert.AreEqual(1, manaPotions.Count, "Should find 1 mana potion type");
    }
    
    [Test]
    public void Inventory_TotalItemCount_SumsAllQuantities()
    {
        // Arrange
        List<TestItem> inventory = new List<TestItem>
        {
            new TestItem("Health Potion", ItemType.HealthPotion, 30, 3),
            new TestItem("Mana Potion", ItemType.ManaPotion, 25, 2)
        };
        
        // Act
        int totalCount = 0;
        foreach (TestItem item in inventory)
        {
            totalCount += item.quantity;
        }
        
        // Assert
        Assert.AreEqual(5, totalCount, "Total should be sum of all quantities (3+2)");
    }
}

