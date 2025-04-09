using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CameraRecoil : MonoBehaviour
{
    public Transform cameraTransform; // Reference to the player's camera
    public float recoilAmount = 2.0f; // How much the camera shakes
    public float recoilSpeed = 10.0f; // How quickly the camera recovers
    public float maxRecoilAngle = 10.0f; // Maximum upward recoil angle

    private Vector3 originalRotation;
    private float currentRecoil;


    void Start()
    {
        if (cameraTransform == null)
        {
            Debug.LogError("Camera Transform is not assigned to CameraRecoil.");
            return;
        }

        originalRotation = cameraTransform.localEulerAngles;
    }

    void Update()
    {
        // Smoothly recover from recoil
        currentRecoil = Mathf.Lerp(currentRecoil, 0, Time.deltaTime * recoilSpeed);
        ApplyRecoilEffect();
    }

    public void AddRecoil(float amount)
    {
        currentRecoil += amount;
        currentRecoil = Mathf.Clamp(currentRecoil, 0, maxRecoilAngle); // Limit the recoil angle
    }

    private void ApplyRecoilEffect()
    {
        // Apply the recoil effect to the camera's rotation
        Vector3 recoilRotation = new Vector3(-currentRecoil, 0, 0); // Recoil affects the X-axis (upward)
        cameraTransform.localEulerAngles = originalRotation + recoilRotation;
    }
}