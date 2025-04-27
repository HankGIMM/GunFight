using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management
using TMPro; // Required for TextMeshPro
using UnityEngine.UI; // Required for UI elements


public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject pauseMenuUI; // Reference to the pause menu UI

    public GameObject playerHUD; // Reference to the PlayerHUD UI
    private bool isPaused = false;


    private void Start()
    {
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
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        playerHUD.SetActive(false); // Hide the PlayerHUD when paused
        Time.timeScale = 0f; // Pause game time
        isPaused = true;

        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true; // Show the cursor
    }

    public void Restart()
    {

        Debug.Log("Restarting the game...");
        Time.timeScale = 1f; // Ensure game time is resumed
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }

    public void QuitToTitle()
    {
        Debug.Log("Returning to the title screen...");
        Time.timeScale = 1f; // Ensure game time is resumed
        SceneManager.LoadScene("TitleScreen"); // Load the title screen
    }
}
