using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tracks game progress and defeated enemies
/// </summary>
public static class GameProgress
{
    // Track which trolls have been defeated
    private static HashSet<int> defeatedTrolls = new HashSet<int>();
    
    // Track if game has been completed
    private static bool gameCompleted = false;
    
    /// <summary>
    /// Mark a troll as defeated
    /// </summary>
    public static void DefeatTroll(int trollIndex)
    {
        defeatedTrolls.Add(trollIndex);
        Debug.Log($"Troll {trollIndex} has been defeated! Defeated trolls: {string.Join(", ", defeatedTrolls)}");
        
        // Check if all trolls are defeated
        if (defeatedTrolls.Count >= 3)
        {
            gameCompleted = true;
            Debug.Log("🎉 ALL TROLLS DEFEATED! Victory flag is now accessible!");
        }
    }
    
    /// <summary>
    /// Check if a specific troll has been defeated
    /// </summary>
    public static bool IsTrollDefeated(int trollIndex)
    {
        return defeatedTrolls.Contains(trollIndex);
    }
    
    /// <summary>
    /// Check if all trolls have been defeated
    /// </summary>
    public static bool IsGameCompleted()
    {
        return gameCompleted;
    }
    
    /// <summary>
    /// Get the number of defeated trolls
    /// </summary>
    public static int GetDefeatedTrollCount()
    {
        return defeatedTrolls.Count;
    }
    
    /// <summary>
    /// Get list of defeated troll indices
    /// </summary>
    public static List<int> GetDefeatedTrolls()
    {
        return new List<int>(defeatedTrolls);
    }
    
    /// <summary>
    /// Reset game progress (for restarting)
    /// </summary>
    public static void ResetProgress()
    {
        defeatedTrolls.Clear();
        gameCompleted = false;
        Debug.Log("Game progress reset - all trolls are alive again!");
    }
    
    /// <summary>
    /// Get victory message for defeated troll
    /// </summary>
    public static string GetVictoryMessage(int trollIndex)
    {
        string trollName = GetTrollName(trollIndex);
        return $"🏆 {trollName} has already been defeated! The area is now safe.";
    }
    
    /// <summary>
    /// Get troll name by index
    /// </summary>
    private static string GetTrollName(int trollIndex)
    {
        switch (trollIndex)
        {
            case 1: return "Troll 1 (Tent)";
            case 2: return "Troll 2 (Round Castle)";
            case 3: return "Troll 3 (Square Castle)";
            default: return "Unknown Troll";
        }
    }
}


