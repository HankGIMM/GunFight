using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    // FOV settings
    protected Camera playerCamera; // Reference to the player's camera
    public float defaultFOV = 60f; // Default FOV
    public float adsFOV = 40f; // FOV when aiming down sights
    public float fovTransitionSpeed = 10f; // Speed of FOV transition

    public Transform defaultPosition;
    public Transform adsPosition;
    public float adsSpeed = 10f;
    protected bool isAiming = false;


    public abstract void Shoot();

    // Method to set the player camera reference
    public void SetPlayerCamera(Camera camera)
    {
        playerCamera = camera;
    }

    public virtual void AimDownSights()
    {
        if (adsPosition != null)
        {
           transform.position = Vector3.Lerp(transform.position, adsPosition.position, Time.deltaTime * adsSpeed);
              transform.rotation = Quaternion.Lerp(transform.rotation, adsPosition.rotation, Time.deltaTime * adsSpeed);
        }

        if (playerCamera != null)
        {
            float targetFOV = isAiming ? adsFOV : defaultFOV;
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * fovTransitionSpeed);
        }
    }

    public virtual void ReturnToDefaultPosition()
    {
        if (defaultPosition != null)
        {
            transform.position = Vector3.Lerp(transform.position, defaultPosition.position, Time.deltaTime * adsSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, defaultPosition.rotation, Time.deltaTime * adsSpeed);
        }

        if (playerCamera != null)
        {
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, defaultFOV, Time.deltaTime * fovTransitionSpeed);
        }
    }
}