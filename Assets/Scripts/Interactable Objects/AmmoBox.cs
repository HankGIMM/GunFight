using UnityEngine;

public class AmmoBox : Interactable
{
    public int ammoAmount = 30; // Amount of ammo the box provides

    public override void Interact()
    {
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.AddAmmo(ammoAmount); // Add ammo to total
            Debug.Log($"Player picked up {ammoAmount} ammo.");
            Destroy(gameObject); // Destroy the ammo box
        }
    }
}