using UnityEngine;

public class PlayerAndroidController : MonoBehaviour
{
    public float moveSpeed = 5f; // Movement speed
    public float lookSpeed = 2f; // Speed for looking around
    public float gravity = 9.8f; // Gravity value

    private Vector2 moveInput; // Input from the joystick
    private Vector2 lookInput; // Input for looking around
    private bool isTouchingJoystick = false; // To track if the joystick is being used
    private bool isLookingAround = false; // To track if the player is looking around

    public RectTransform joystickHandle; // Joystick handle (UI element)
    public RectTransform joystickBackground; // Joystick background (UI element)
    public Transform playerCamera; // Player's camera for looking around

    private Vector3 velocity; // Player's movement and gravity velocity
    private CharacterController characterController; // Reference to the CharacterController

    private void Start()
    {
        // Ensure there's a CharacterController attached
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("No CharacterController found! Please attach a CharacterController to the player.");
        }

        // Ensure the player's camera is assigned
        if (playerCamera == null)
        {
            Debug.LogError("No Camera assigned! Please assign the playerCamera in the Inspector.");
        }
    }

    private void Update()
    {
        HandleMovementInput();
        HandleLookInput();
        ApplyGravity();
        MovePlayer();
    }

    private void HandleMovementInput()
    {
        if (Input.GetMouseButtonDown(0)) // Detect touch or click
        {
            Vector2 touchPosition = Input.mousePosition;
            if (RectTransformUtility.RectangleContainsScreenPoint(joystickBackground, touchPosition))
            {
                isTouchingJoystick = true;
            }
        }

        if (Input.GetMouseButton(0) && isTouchingJoystick)
        {
            Vector2 touchPosition = Input.mousePosition;
            Vector2 joystickCenter = joystickBackground.position;

            moveInput = (touchPosition - joystickCenter) / (joystickBackground.sizeDelta.x * 0.5f);

            if (moveInput.magnitude > 1f)
                moveInput = moveInput.normalized;

            // Update joystick handle position
            joystickHandle.anchoredPosition = moveInput * (joystickBackground.sizeDelta.x * 0.4f);
        }

        if (Input.GetMouseButtonUp(0) && isTouchingJoystick)
        {
            isTouchingJoystick = false;
            moveInput = Vector2.zero;
            joystickHandle.anchoredPosition = Vector2.zero;
        }
    }

    private void HandleLookInput()
    {
        if (Input.GetMouseButtonDown(0) && !isTouchingJoystick)
        {
            isLookingAround = true;
        }

        if (Input.GetMouseButton(0) && isLookingAround)
        {
            lookInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            // Rotate the player horizontally
            transform.Rotate(Vector3.up * lookInput.x * lookSpeed);

            // Rotate the camera vertically
            float pitch = playerCamera.localEulerAngles.x - lookInput.y * lookSpeed;
            pitch = Mathf.Clamp(pitch > 180 ? pitch - 360 : pitch, -75, 75); // Clamp to avoid over-rotation
            playerCamera.localEulerAngles = new Vector3(pitch, 0, 0);
        }

        if (Input.GetMouseButtonUp(0) && isLookingAround)
        {
            isLookingAround = false;
        }
    }

    private void MovePlayer()
    {
        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y) * moveSpeed;

        // Convert movement to world space relative to the camera
        movement = transform.TransformDirection(movement);

        // Apply gravity to the vertical component
        movement.y = velocity.y;

        // Move the player using CharacterController
        characterController.Move(movement * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        if (characterController.isGrounded)
        {
            velocity.y = -2f; // Small downward force to keep grounded
        }
        else
        {
            velocity.y -= gravity * Time.deltaTime; // Apply gravity
        }
    }
}