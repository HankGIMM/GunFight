using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10.0f;
    public CharacterController controller;
    public float mouseSensitivity = 2.0f;
    public Transform playerCamera;
    private float verticalRotation = 0;

    public float interactionDistance = 5.0f;

    public Weapon equippedWeapon;

    private float gravity = -9.81f;
    private Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMouseLook();
        HandleMovement();
        HandleInteraction();
        HandleShooting();
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        float moveForwardBackward = Input.GetAxis("Vertical");
        float moveLeftRight = Input.GetAxis("Horizontal");

        Vector3 move = transform.right * moveLeftRight + transform.forward * moveForwardBackward;
        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(playerCamera.position, playerCamera.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, interactionDistance))
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    interactable.Interact();
                }
            }
        }
    }

    void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            if (equippedWeapon != null)
            {
                equippedWeapon.Shoot();
            }
        }
    }
}
