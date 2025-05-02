using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public abstract class Interactable : MonoBehaviour
{
    public string interactionMessage; // Message to display when the player is near
    private Material originalMaterial;
    private Material highlightMaterial;
    private PlayerHUD playerHUD;
    //public PlayerController playerController;

    private Renderer objectRenderer;

    private void Start()
    {
        playerHUD = FindObjectOfType<PlayerHUD>();

        if (playerHUD == null)
        {
            Debug.LogError("PlayerHUD not found in the scene.");
        }

        //find the renderer component
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer == null)
        {
            objectRenderer = GetComponentInChildren<Renderer>();
        }

        if (objectRenderer != null)
        {
            originalMaterial = objectRenderer.material;
        }
        else
        {
            Debug.LogWarning($"No Renderer found on {gameObject.name} or its children.");
        }
    }

    public abstract void Interact(PlayerController playerController);

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && playerHUD != null)
        {
            Debug.Log($"Player entered interaction range of {gameObject.name}");
            playerHUD.ShowPickupPrompt(interactionMessage); // Show the pickup prompt
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && playerHUD != null)
        {
            Debug.Log($"Player exited interaction range of {gameObject.name}");
            playerHUD.HidePickupPrompt(); // Hide the pickup prompt
        }
    }

    public void Highlight()
    {
        if (objectRenderer != null)
        {
            objectRenderer.material.color = Color.yellow; // Highlight the object
        }
    }

    public void Unhighlight()
    {
         if (objectRenderer != null && originalMaterial != null)
        {
            objectRenderer.material = originalMaterial; // Restore the original material
        }
    }
   
}