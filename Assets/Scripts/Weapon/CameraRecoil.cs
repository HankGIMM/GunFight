using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CameraRecoil : MonoBehaviour
{
    public Transform cameraTransform; // Reference to the player's camera
    public float recoilAmount = 2.0f; // How much the camera shakes
    public float recoilSpeed = 10.0f; // How quickly the camera recovers
    public float maxRecoilAngle = 10.0f; // Maximum upward recoil angle

    public float currentRecoil;

    void Update()
    {
        // Smoothly recover from recoil
        currentRecoil = Mathf.Lerp(currentRecoil, 0, Time.deltaTime * recoilSpeed);
    }

    public void AddRecoil(float amount)
    {
        currentRecoil += amount;
        currentRecoil = Mathf.Clamp(currentRecoil, 0, maxRecoilAngle); // Limit the recoil angle
    }

    public float GetRecoilOffset()
    {
        return currentRecoil;
    }
}