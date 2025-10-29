using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Simple GameOverManager - guaranteed to work
/// </summary>
public class SimpleGameOverManager : MonoBehaviour
{
    public string mainMenuSceneName = "MainMenuScene";

    public void GoToMainMenu()
    {
        Debug.LogError("SimpleGameOverManager: MAIN MENU CLICKED!");
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void RestartBattle()
    {
        Debug.LogError("SimpleGameOverManager: RESTART BATTLE CLICKED!");
        SceneManager.LoadScene("BattleScene");
    }

    public void QuitGame()
    {
        Debug.LogError("SimpleGameOverManager: QUIT GAME CLICKED!");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void TestClick()
    {
        Debug.LogError("SimpleGameOverManager: TEST CLICKED!");
    }
}
