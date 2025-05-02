using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AmmoBox : Interactable
{

    public int ammoAmount = 30; // Amount of ammo the box provides

    private void Start()
    {
        interactionMessage = "Press E to pick up ammo"; // Set the custom interaction message
    }
    public override void Interact(PlayerController playerController)
    {
        //PlayerController player = FindObjectOfType<PlayerController>();
        if (playerController != null && playerController.equippedWeapon is Gun gun)
        {
            // Add ammo to the player's weapon
            gun.totalAmmo += ammoAmount;
            Debug.Log($"Player picked up {ammoAmount} ammo. Total Ammo: {gun.totalAmmo}");

            // Update the HUD
            PlayerHUD playerHUD = FindObjectOfType<PlayerHUD>();
            if (playerHUD != null)
            {
                playerHUD.UpdateAmmoUI();
            }

            // Destroy the ammo box
            Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning("Player or equipped weapon not found.");
        }
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         Interact(); // Automatically interact when the player collides with the ammo box
    //     }
    // }
}