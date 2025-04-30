using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class GameSceneManager : MonoBehaviour
{
    public string mainSceneName = "Main"; // Name of the main scene to load
    public string titleSceneName = "Title"; // Name of the title screen scene

    // Method to start the game
    public void StartGame()
    {
        Debug.Log("Starting the game...");
        SceneManager.LoadScene(mainSceneName); // Load the main scene

    }

    // Method to quit the game
    public void QuitGame()
    {
        Debug.Log("QuitGame method called.");
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false; // Stop play mode in the Editor
#else
        Application.Quit(); // Quit the application
#endif
    }

    // Method to return to the title screen
    public void ReturnToTitle()
    {
        Debug.Log("Returning to the title screen...");
        SceneManager.LoadScene(titleSceneName); // Load the title screen scene
    }

    // Method to restart the current scene
    public void RestartScene()
    {
        Debug.Log("Restarting the current scene...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }
}
