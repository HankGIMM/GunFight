using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10.0f;
    public float mouseSensitivity = 2.0f;
    public Transform playerCamera;
    private float verticalRotation = 0;
    // Start is called before the first frame update
    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;


    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        transform.Rotate(Vector3.up * mouseX);

        // Handle movement
        float moveForwardBackward = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float moveLeftRight = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

        Vector3 move = transform.right * moveLeftRight + transform.forward * moveForwardBackward;
        transform.position += move;
    }
}
