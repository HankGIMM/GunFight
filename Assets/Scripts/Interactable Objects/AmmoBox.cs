using UnityEngine;
using System.Collections;

public class AmmoBox : Interactable
{

    public int ammoAmount = 30; // Amount of ammo the box provides
    
    // Reference to the PlayerController
    public PlayerController playerController;

    public override void Interact()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
           // player.AddAmmo(ammoAmount); // Add ammo to total
            Debug.Log($"Player picked up {ammoAmount} ammo.");
            Destroy(gameObject); // Destroy the ammo box
        }
        else
        {
            Debug.LogWarning("Player not found.");
        }
    }
}