using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public string mainSceneName = "MainScene"; // Name of the main scene to load
    public string titleSceneName = "TitleScreen"; // Name of the title screen scene

    // Method to start the game
    public void StartGame()
    {
        Debug.Log("Starting the game...");
        SceneManager.LoadScene(mainSceneName); // Load the main scene
    }

    // Method to quit the game
    public void QuitGame()
    {
        Debug.Log("Quitting the game...");
        Application.Quit(); // Quit the application
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
