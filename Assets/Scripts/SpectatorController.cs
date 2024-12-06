using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpectatorController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintMultiplier = 2f;
    [SerializeField] private float lookSensitivity = 2f;
    [SerializeField] private KeyCode toggleKey = KeyCode.Tab;
    [SerializeField] private Image cameraImage;


    private bool isSpectatorModeActive = false;
    private Rigidbody rb;
    private Vector3 movement;
    private float pitch = 0f; // Up and down rotation
    private float yaw = 0f;  // Left and right rotation

    private Quaternion initialRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //extra safety
        rb.useGravity = false;  // Disable gravity for a spectator
        rb.isKinematic = false; // Allow physics interaction
        
        // Capture the initial camera rotation
        initialRotation = transform.rotation;

        // Initialize yaw and pitch based on the initial rotation
        Vector3 eulerAngles = initialRotation.eulerAngles;
        yaw = eulerAngles.y;
        pitch = eulerAngles.x;
        if (!isSpectatorModeActive)
            cameraImage.color = Color.red;
        else 
            cameraImage.color = Color.white;
    }

    void Update()
    {
        // Toggle spectator mode
        if (Input.GetKeyDown(toggleKey))
        {
            isSpectatorModeActive = !isSpectatorModeActive;
            Cursor.lockState = isSpectatorModeActive ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !isSpectatorModeActive;
            cameraImage.color = Color.white;
            
            //Bug found during the tests
            if (!isSpectatorModeActive)
            {
                // Prevent spinning: lock the current rotation when exiting
                transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
                cameraImage.color = Color.red;
            }
        }
    }

    void FixedUpdate()
    {
        if (isSpectatorModeActive)
        {
            HandleMovement();
            HandleRotation();
        }
        else
        {
            rb.velocity = new Vector3(0, 0, 0);
        }
    }

    private void HandleMovement()
    {
        float speed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift)) speed *= sprintMultiplier;

        movement = Vector3.zero;

        // WASD Movement
        if (Input.GetKey(KeyCode.W)) movement += transform.forward;
        if (Input.GetKey(KeyCode.S)) movement -= transform.forward;
        if (Input.GetKey(KeyCode.A)) movement -= transform.right;
        if (Input.GetKey(KeyCode.D)) movement += transform.right;

        // Space and Ctrl for vertical movement
        if (Input.GetKey(KeyCode.Space)) movement += transform.up;
        if (Input.GetKey(KeyCode.LeftControl)) movement -= transform.up;

        rb.velocity = movement.normalized * speed; // Apply velocity for physics-based movement
    }

    private void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;

        yaw += mouseX;
        pitch -= mouseY;

        pitch = Mathf.Clamp(pitch, -90f, 90f);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}
