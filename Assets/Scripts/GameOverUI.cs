using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required for UI elements
using TMPro; // Required for TextMeshPro
using UnityEngine.SceneManagement; // Required for scene management
using UnityEngine.Audio; // Required for audio management


public class GameOverUI : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject winScreen; // Reference to the Win screen UI
    public GameObject loseScreen; // Reference to the Lose screen UI

    [Header("Audio Clips")]
    public AudioClip victoryMusic; // Music to play on victory
    public AudioClip loseMusic; // Music to play on defeat



    private void Start()
    {
        // Ensure both screens are hidden at the start
        if (winScreen != null) winScreen.SetActive(false);
        if (loseScreen != null) loseScreen.SetActive(false);
    }

    public void ShowWinScreen()
    {
        Debug.Log("Win screen called");
        if (winScreen != null)
        {
            winScreen.SetActive(true);
            Time.timeScale = 0f; // Pause the game
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor
            Cursor.visible = true; // Show the cursor

            // Play victory music
            if (victoryMusic != null)
            {
                AudioManager.Instance.PlayMusic(victoryMusic);
            }
            else
            {
                Debug.LogError("Victory music is not assigned in the Inspector.");
            }
        }
        else
        {
            Debug.LogError("Win screen is not assigned in the Inspector.");
        }
    }

    public void ShowLoseScreen()
    {
        if (loseScreen != null)
        {
            loseScreen.SetActive(true);
            Time.timeScale = 0f; // Pause the game
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor
            Cursor.visible = true; // Show the cursor

            // Play lose music
            if (loseMusic != null)
            {
                AudioManager.Instance.PlayMusic(loseMusic);
            }
            else
            {
                Debug.LogError("Lose music is not assigned in the Inspector.");
            }
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Resume the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }

    public void QuitToTitle()
    {
        Time.timeScale = 1f; // Resume the game
        SceneManager.LoadScene("TitleScreen"); // Load the title screen
    }
}
