using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public float speed = 10.0f;
    public CharacterController controller;
    public float mouseSensitivity = 2.0f;
    public Transform playerCamera;
    private float verticalRotation = 0;

    public float interactionDistance = 5.0f;

    public Weapon equippedWeapon;

    public float gravity;
    private Vector3 velocity;

    public float jumpHeight = 3.0f;
    private bool isGrounded;

    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;

    private Interactable interactable;

    public float Health = 100f;

    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.IsGamePaused || GameManager.Instance.gameOver) return; // Skip update if the game is paused
        
        HandleMouseLook();
        HandleMovement();
        HandleInteraction();
        // HandleShooting();

    }

    void HandleMouseLook()
    {
        if (PauseMenu.IsGamePaused) return; // Skip mouse look if the game is paused
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Adjust vertical rotation (looking up and down)
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f); // Prevent over-rotation

        // Apply vertical rotation to the camera, including recoil
        float recoilOffset = playerCamera.GetComponent<CameraRecoil>()?.currentRecoil ?? 0;
        playerCamera.localRotation = Quaternion.Euler(verticalRotation - recoilOffset, 0, 0);

        // Apply horizontal rotation to the player body
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float moveForwardBackward = Input.GetAxis("Vertical");
        float moveLeftRight = Input.GetAxis("Horizontal");

        Vector3 move = transform.right * moveLeftRight + transform.forward * moveForwardBackward;
        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleInteraction()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();

            if (interactable != null)
            {
                // Highlight the interactable object
                interactable.Highlight();

                // Check if the player presses the interaction key
                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactable.Interact();
                }
            }
            else
            {
                if (interactable != null)
                {
                    interactable.Unhighlight();
                    interactable = null;
                }

            }
        }
    }


    // void HandleShooting()
    // {
    //     if (Input.GetMouseButtonDown(0)) // Left mouse button
    //     {
    //         if (equippedWeapon != null)
    //         {
    //             equippedWeapon.Shoot();
    //         }
    //     }
    // }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        Debug.Log($"Player took {damage} damage. Remaining Health: {Health}");

        if (Health <= 0)
        {
            Debug.Log("Player is dead!");
            GameManager.Instance?.gameOverUI?.ShowLoseScreen(); // Notify player death

        }
    }

}
