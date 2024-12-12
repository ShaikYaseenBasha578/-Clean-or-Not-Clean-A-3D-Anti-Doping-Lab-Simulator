using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This Will Auto Add Character Controller To Gameobject If It's Not Already Applied:
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    // Camera:
    public Camera playerCam;

    // Movement Settings:
    public float walkSpeed = 3f;
    public float runSpeed = 5f;
    public float jumpPower = 0f;
    public float gravity = 10f;

    // Camera Settings:
    public float lookSpeed = 2f;
    public float lookXLimit = 75f;
    public float cameraRotationSmooth = 5f;



    private bool isWalking = false;
    private bool isFootstepCoroutineRunning = false;
    private AudioClip[] currentFootstepSounds;

    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;
    float rotationY = 0;

    // Camera Zoom Settings:
    public int ZoomFOV = 35;
    public int initialFOV;
    public float cameraZoomSmooth = 1;

    private bool isZoomed = false;

    // Can The Player Move?:
    private bool canMove = true;

    CharacterController characterController;

    void Start()
    {
        // Ensure We Are Using The Character Controller Component:
        characterController = GetComponent<CharacterController>();

        // Lock And Hide Cursor:
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


    }

    void Update()
    {
        // Walking/Running In Action:
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // Jumping In Action:
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);

        // Camera Movement In Action:
        if (canMove)
        {
            rotationX -= Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

            rotationY += Input.GetAxis("Mouse X") * lookSpeed;

            Quaternion targetRotationX = Quaternion.Euler(rotationX, 0, 0);
            Quaternion targetRotationY = Quaternion.Euler(0, rotationY, 0);

            playerCam.transform.localRotation = Quaternion.Slerp(playerCam.transform.localRotation, targetRotationX, Time.deltaTime * cameraRotationSmooth);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotationY, Time.deltaTime * cameraRotationSmooth);
        }

        // Zooming In Action:
        if (Input.GetButtonDown("Fire2"))
        {
            isZoomed = true;
        }

        if (Input.GetButtonUp("Fire2"))
        {
            isZoomed = false;
        }

        if (isZoomed)
        {
            playerCam.GetComponent<Camera>().fieldOfView = Mathf.Lerp(playerCam.fieldOfView, ZoomFOV, Time.deltaTime * cameraZoomSmooth);
        }
        else
        {
            playerCam.GetComponent<Camera>().fieldOfView = Mathf.Lerp(playerCam.fieldOfView, initialFOV, Time.deltaTime * cameraZoomSmooth);
        }




    }

}