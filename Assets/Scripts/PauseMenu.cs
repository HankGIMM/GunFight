using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management
using TMPro; // Required for TextMeshPro
using UnityEngine.UI; // Required for UI elements


public class PauseMenu : MonoBehaviour
{
    public static bool IsGamePaused = false; // Static variable to track if the game is paused

    public GameObject pauseMenuUI; // Reference to the pause menu UI

    public GameObject playerHUD; // Reference to the PlayerHUD UI
    private bool isPaused = false;


    private void Start()
    {
        // Reassign references to ensure they are valid after a scene reload
       // pauseMenuUI = GameObject.Find("PauseMenuUI"); // Replace with the actual name of your Pause Menu UI GameObject
       // playerHUD = GameObject.Find("PlayerHUD"); // Replace with the actual name of your Player HUD GameObject

        if (pauseMenuUI == null)
        {
            Debug.LogError("PauseMenuUI is not assigned in the Inspector.");
        }

        if (playerHUD == null)
        {
            Debug.LogError("PlayerHUD is not assigned in the Inspector.");
        }

        pauseMenuUI.SetActive(false); // Ensure the pause menu is hidden at the start
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Press Escape to toggle pause
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        playerHUD.SetActive(true); // Show the PlayerHUD when resuming
        Time.timeScale = 1f; // Resume game time
        isPaused = false;
        IsGamePaused = false; // Update the static variable

        AudioManager.Instance.TransitionToSnapshot(AudioManager.Instance.defaultSnapshot, 0.5f); // Resume gameplay music

        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
        Cursor.visible = false; // Hide the cursor
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        playerHUD.SetActive(false); // Hide the PlayerHUD when paused
        Time.timeScale = 0f; // Pause game time
        isPaused = true;
        IsGamePaused = true; // Update the static variable

        AudioManager.Instance.TransitionToSnapshot(AudioManager.Instance.pausedSnapshot, 0.5f);

        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true; // Show the cursor
    }

    public void Restart()
    {

        Debug.Log("Restarting the game...");
        GameSceneManager.Instance.RestartScene(); // Call the RestartScene method from GameSceneManager

    }

    public void QuitToTitle()
    {
        Debug.Log("Returning to the title screen...");
        GameSceneManager.Instance.ReturnToTitle(); // Call the ReturnToTitle method from GameSceneManager
    }
}
