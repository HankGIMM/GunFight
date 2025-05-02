using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management
using TMPro; // Required for TextMeshPro
using UnityEngine.UI; // Required for UI elements


public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance; // Singleton instance
    public string mainSceneName = "Main"; // Name of the main scene to load
    public string titleSceneName = "Title"; // Name of the title screen scene

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // Set the singleton instance
            //DontDestroyOnLoad(gameObject); // Keep this object across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    // Method to start the game
    public void StartGame()
    {
        Debug.Log("Starting the game...");
        Time.timeScale = 1f; // Ensure game time is resumed
        AudioManager.Instance.ReloadAudioClips(); // Reload audio clips
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Load the main scene

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
        Time.timeScale = 1f; // Ensure game time is resumed
        AudioManager.Instance.ReloadAudioClips(); // Reload audio clips
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1); // Load the title screen scene
    }

    // Method to restart the current scene
    public void RestartScene()
    {
        Debug.Log("Restarting the current scene..." + SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f; // Ensure game time is resumed
        AudioManager.Instance.ReloadAudioClips(); // Reload audio clips
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene

    }
}
