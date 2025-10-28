using UnityEngine;

/// <summary>
/// Helper script to test AudioManager functionality
/// Attach this to any GameObject to test audio in Play mode
/// </summary>
public class AudioManagerSetup : MonoBehaviour
{
    [Header("Test Controls")]
    [Tooltip("Press these keys in Play mode to test sounds")]
    public KeyCode testAttackKey = KeyCode.Alpha1;
    public KeyCode testHitKey = KeyCode.Alpha2;
    public KeyCode testSuperAttackKey = KeyCode.Alpha3;
    public KeyCode testVictoryKey = KeyCode.Alpha4;
    public KeyCode testDefeatKey = KeyCode.Alpha5;
    
    void Update()
    {
        // Test sound effects with keyboard
        if (AudioManager.Instance != null)
        {
            if (Input.GetKeyDown(testAttackKey))
            {
                AudioManager.Instance.PlayAttackSound();
                Debug.Log("Testing Attack Sound (Key: 1)");
            }
            
            if (Input.GetKeyDown(testHitKey))
            {
                AudioManager.Instance.PlayHitSound();
                Debug.Log("Testing Hit Sound (Key: 2)");
            }
            
            if (Input.GetKeyDown(testSuperAttackKey))
            {
                AudioManager.Instance.PlaySuperAttackSound();
                Debug.Log("Testing Super Attack Sound (Key: 3)");
            }
            
            if (Input.GetKeyDown(testVictoryKey))
            {
                AudioManager.Instance.PlayVictorySound();
                Debug.Log("Testing Victory Sound (Key: 4)");
            }
            
            if (Input.GetKeyDown(testDefeatKey))
            {
                AudioManager.Instance.PlayDefeatSound();
                Debug.Log("Testing Defeat Sound (Key: 5)");
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                Debug.LogWarning("AudioManager.Instance is null! Make sure AudioManager is in the scene.");
            }
        }
    }
    
    void OnGUI()
    {
        // Show instructions on screen
        GUI.Label(new Rect(10, 10, 300, 200), 
            "AUDIO MANAGER TEST:\n" +
            "1 - Attack Sound\n" +
            "2 - Hit Sound\n" +
            "3 - Super Attack Sound\n" +
            "4 - Victory Sound\n" +
            "5 - Defeat Sound\n" +
            "H - Check AudioManager Status");
    }
}
