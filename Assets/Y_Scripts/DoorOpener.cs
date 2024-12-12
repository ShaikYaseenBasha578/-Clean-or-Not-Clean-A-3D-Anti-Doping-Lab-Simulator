using UnityEngine;
using System.Collections;

public class DoorOpener : MonoBehaviour
{
    public Transform door; // The door's Transform
    public Vector3 openRotation = new Vector3(0, 90, 0); // Rotation for the open state (relative to the closed rotation)
    public float openSpeed = 2f; // Speed of the door opening
    public bool autoClose = true; // Should the door close automatically?
    public float autoCloseDelay = 5f; // Delay before the door closes automatically

    private Quaternion closedRotation; // The original rotation of the door
    private Quaternion targetRotation; // The current target rotation of the door
    private bool isOpen = false; // Is the door open?
    private Coroutine autoCloseCoroutine;

    private void Start()
    {
        // Save the initial rotation as the closed rotation
        if (door != null)
        {
            closedRotation = door.rotation;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OpenDoor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && autoClose)
        {
            StartAutoClose();
        }
    }

    private void Update()
    {
        // Smoothly interpolate the door's rotation towards the target rotation
        if (door != null)
        {
            door.rotation = Quaternion.Slerp(door.rotation, targetRotation, Time.deltaTime * openSpeed);
        }
    }

    private void OpenDoor()
    {
        if (!isOpen && door != null)
        {
            targetRotation = closedRotation * Quaternion.Euler(openRotation); // Set target to open rotation
            isOpen = true;
        }
    }

    private void CloseDoor()
    {
        if (isOpen && door != null)
        {
            targetRotation = closedRotation; // Set target to closed rotation
            isOpen = false;
        }
    }

    private void StartAutoClose()
    {
        if (autoCloseCoroutine != null)
        {
            StopCoroutine(autoCloseCoroutine);
        }
        autoCloseCoroutine = StartCoroutine(AutoCloseRoutine());
    }

    private IEnumerator AutoCloseRoutine()
    {
        yield return new WaitForSeconds(autoCloseDelay);
        CloseDoor();
    }
}

