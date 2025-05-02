using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
     public float mouseSensitivity = 2.0f; // Sensitivity for mouse movement
    private float verticalRotation = 0.0f;

    void Update()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Adjust vertical rotation (looking up and down)
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f); // Prevent over-rotation

        // Apply vertical rotation to the camera
        transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

        // Apply horizontal rotation to the mortar camera's parent (if needed)
        transform.parent.Rotate(Vector3.up * mouseX);
    }
}
