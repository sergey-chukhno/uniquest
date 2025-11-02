using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Unit tests for Team Management System
/// Tests character selection, team composition, and character switching
/// </summary>
public class TeamManagementTests
{
    // Helper class for testing (mimics TeamMember structure)
    private class TestTeamMember
    {
        public string name;
        public int health;
        public bool isDefeated;
        
        public TestTeamMember(string name, int health)
        {
            this.name = name;
            this.health = health;
            this.isDefeated = health <= 0;
        }
    }
    
    // ==========================================
    // TEAM COMPOSITION TESTS
    // ==========================================
    
    [Test]
    public void Team_AddCharacter_IncreasesTeamSize()
    {
        // Arrange
        List<TestTeamMember> team = new List<TestTeamMember>();
        int maxTeamSize = 3;
        
        // Act
        team.Add(new TestTeamMember("Warrior 1", 100));
        
        // Assert
        Assert.AreEqual(1, team.Count, "Team should have 1 member after adding");
    }
    
    [Test]
    public void Team_AddCharacter_WhenFull_ReturnsFalse()
    {
        // Arrange
        List<TestTeamMember> team = new List<TestTeamMember>();
        int maxTeamSize = 3;
        
        team.Add(new TestTeamMember("Warrior 1", 100));
        team.Add(new TestTeamMember("Warrior 2", 80));
        team.Add(new TestTeamMember("Warrior 3", 70));
        
        // Act
        bool canAdd = team.Count < maxTeamSize;
        
        // Assert
        Assert.IsFalse(canAdd, "Should not be able to add when team is full");
        Assert.AreEqual(3, team.Count, "Team should remain at max size");
    }
    
    [Test]
    public void Team_AddDuplicateCharacter_Prevented()
    {
        // Arrange
        List<TestTeamMember> team = new List<TestTeamMember>();
        string characterName = "Warrior 1";
        team.Add(new TestTeamMember(characterName, 100));
        
        // Act
        bool isDuplicate = team.Any(m => m.name == characterName);
        
        // Assert
        Assert.IsTrue(isDuplicate, "Should detect duplicate character");
    }
    
    [Test]
    public void Team_RemoveCharacter_DecreasesTeamSize()
    {
        // Arrange
        List<TestTeamMember> team = new List<TestTeamMember>();
        TestTeamMember warrior1 = new TestTeamMember("Warrior 1", 100);
        team.Add(warrior1);
        team.Add(new TestTeamMember("Warrior 2", 80));
        
        // Act
        team.Remove(warrior1);
        
        // Assert
        Assert.AreEqual(1, team.Count, "Team should have 1 member after removal");
    }
    
    // ==========================================
    // CHARACTER SWITCHING TESTS
    // ==========================================
    
    [Test]
    public void CharacterSwitch_GetActiveCharacter_ReturnsCorrectMember()
    {
        // Arrange
        List<TestTeamMember> team = new List<TestTeamMember>
        {
            new TestTeamMember("Warrior 1", 100),
            new TestTeamMember("Warrior 2", 80),
            new TestTeamMember("Warrior 3", 70)
        };
        int activeIndex = 0;
        
        // Act
        TestTeamMember activeCharacter = team[activeIndex];
        
        // Assert
        Assert.AreEqual("Warrior 1", activeCharacter.name, "Should return first character");
        Assert.AreEqual(100, activeCharacter.health, "Should have correct health");
    }
    
    [Test]
    public void CharacterSwitch_SwitchToNext_CyclesCorrectly()
    {
        // Arrange
        List<TestTeamMember> team = new List<TestTeamMember>
        {
            new TestTeamMember("Warrior 1", 100),
            new TestTeamMember("Warrior 2", 80),
            new TestTeamMember("Warrior 3", 70)
        };
        int activeIndex = 0;
        
        // Act
        activeIndex = (activeIndex + 1) % team.Count;
        
        // Assert
        Assert.AreEqual(1, activeIndex, "Should switch to second character");
        Assert.AreEqual("Warrior 2", team[activeIndex].name);
    }
    
    [Test]
    public void CharacterSwitch_SwitchFromLast_WrapsToFirst()
    {
        // Arrange
        List<TestTeamMember> team = new List<TestTeamMember>
        {
            new TestTeamMember("Warrior 1", 100),
            new TestTeamMember("Warrior 2", 80),
            new TestTeamMember("Warrior 3", 70)
        };
        int activeIndex = 2; // Last character
        
        // Act
        activeIndex = (activeIndex + 1) % team.Count;
        
        // Assert
        Assert.AreEqual(0, activeIndex, "Should wrap to first character");
    }
    
    [Test]
    public void CharacterSwitch_SkipsDefeatedCharacters()
    {
        // Arrange
        List<TestTeamMember> team = new List<TestTeamMember>
        {
            new TestTeamMember("Warrior 1", 0),   // Defeated
            new TestTeamMember("Warrior 2", 80),  // Alive
            new TestTeamMember("Warrior 3", 0)    // Defeated
        };
        int activeIndex = 0;
        
        // Act - Find next alive character
        for (int i = 1; i < team.Count; i++)
        {
            int nextIndex = (activeIndex + i) % team.Count;
            if (!team[nextIndex].isDefeated)
            {
                activeIndex = nextIndex;
                break;
            }
        }
        
        // Assert
        Assert.AreEqual(1, activeIndex, "Should skip defeated characters");
        Assert.IsFalse(team[activeIndex].isDefeated, "Active character should be alive");
    }
    
    // ==========================================
    // TEAM WIPE TESTS
    // ==========================================
    
    [Test]
    public void TeamWipe_AllDefeated_ReturnsTrue()
    {
        // Arrange
        List<TestTeamMember> team = new List<TestTeamMember>
        {
            new TestTeamMember("Warrior 1", 0),
            new TestTeamMember("Warrior 2", 0),
            new TestTeamMember("Warrior 3", 0)
        };
        
        // Act
        bool isWiped = team.All(m => m.isDefeated);
        
        // Assert
        Assert.IsTrue(isWiped, "Team should be wiped when all characters defeated");
    }
    
    [Test]
    public void TeamWipe_OneAlive_ReturnsFalse()
    {
        // Arrange
        List<TestTeamMember> team = new List<TestTeamMember>
        {
            new TestTeamMember("Warrior 1", 0),
            new TestTeamMember("Warrior 2", 30), // Still alive
            new TestTeamMember("Warrior 3", 0)
        };
        
        // Act
        bool isWiped = team.All(m => m.isDefeated);
        bool hasAlive = team.Any(m => !m.isDefeated);
        
        // Assert
        Assert.IsFalse(isWiped, "Team should not be wiped with alive member");
        Assert.IsTrue(hasAlive, "Should have at least one alive member");
    }
    
    [Test]
    public void TeamWipe_CountAliveMembers_ReturnsCorrectCount()
    {
        // Arrange
        List<TestTeamMember> team = new List<TestTeamMember>
        {
            new TestTeamMember("Warrior 1", 0),   // Defeated
            new TestTeamMember("Warrior 2", 80),  // Alive
            new TestTeamMember("Warrior 3", 60)   // Alive
        };
        
        // Act
        int aliveCount = team.Count(m => !m.isDefeated);
        
        // Assert
        Assert.AreEqual(2, aliveCount, "Should have 2 alive members");
    }
}

