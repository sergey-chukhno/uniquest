using UnityEngine;

/// <summary>
/// Simple test script to verify GameOverManager is working
/// </summary>
public class GameOverTest : MonoBehaviour
{
    void Start()
    {
        Debug.LogError("GameOverTest: Start() called - Testing GameOverManager...");
        
        // Find GameOverManager
        GameOverManager gameOverManager = FindObjectOfType<GameOverManager>();
        if (gameOverManager != null)
        {
            Debug.LogError("GameOverTest: GameOverManager found!");
            
            // Test the public methods
            gameOverManager.ShowGameOver("Test Zone", 1);
            gameOverManager.ForceReinitialize();
        }
        else
        {
            Debug.LogError("GameOverTest: GameOverManager NOT FOUND!");
        }
    }
}
