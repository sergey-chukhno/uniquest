using UnityEngine;

/// <summary>
/// Helper script to test the Game Over screen
/// Attach to any GameObject and press G key to trigger game over
/// </summary>
public class GameOverTester : MonoBehaviour
{
    [Header("Test Settings")]
    public GameOverManager gameOverManager;
    public KeyCode testKey = KeyCode.G;
    
    void Update()
    {
        // Test game over screen with G key
        if (Input.GetKeyDown(testKey))
        {
            TestGameOver();
        }
    }
    
    void TestGameOver()
    {
        if (gameOverManager != null)
        {
            Debug.Log("GameOverTester: Testing Game Over screen");
            
            // Test with sample data
            string testBattlezone = "Test Battle Zone";
            int testDefeatedTrolls = GameProgress.GetDefeatedTrollCount();
            
            gameOverManager.ShowGameOver(testBattlezone, testDefeatedTrolls);
        }
        else
        {
            Debug.LogWarning("GameOverTester: No GameOverManager assigned!");
        }
    }
    
    void OnGUI()
    {
        // Show instructions on screen
        GUI.Label(new Rect(10, 100, 300, 50), 
            $"GAME OVER TEST:\nPress '{testKey}' to test Game Over screen");
    }
}
