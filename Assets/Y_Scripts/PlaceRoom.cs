using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class PlaceRoom : MonoBehaviour
{
    public GameObject roomPrefab; // Assign the room prefab in the Inspector
    private ARRaycastManager raycastManager;
    private ARAnchorManager anchorManager; // For anchoring the room

    private GameObject spawnedRoom; // Reference to the instantiated room
    private ARAnchor currentAnchor; // Anchor for the spawned room

    void Start()
    {
        // Find and assign AR Managers
        raycastManager = FindObjectOfType<ARRaycastManager>();
        anchorManager = FindObjectOfType<ARAnchorManager>();

        if (raycastManager == null)
        {
            Debug.LogError("ARRaycastManager not found in the scene.");
        }

        if (anchorManager == null)
        {
            Debug.LogError("ARAnchorManager not found in the scene.");
        }

        if (roomPrefab == null)
        {
            Debug.LogError("Room prefab is not assigned.");
        }
    }

    [System.Obsolete]
    void Update()
    {
        // Check for touch input
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector2 touchPosition = Input.GetTouch(0).position;

            // Perform a raycast to detect planes at the touch position
            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            if (raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
            {
                // Get the pose of the hit
                Pose hitPose = hits[0].pose;

                // Destroy the previous room and its anchor if they exist
                if (currentAnchor != null)
                {
                    Destroy(currentAnchor.gameObject);
                    Debug.Log("Previous anchor and room destroyed.");
                }

                // Create a new anchor at the hit position
                currentAnchor = anchorManager.AddAnchor(hitPose);
                if (currentAnchor == null)
                {
                    Debug.LogError("Failed to create anchor.");
                    return;
                }

                // Instantiate the room prefab and parent it to the anchor
                spawnedRoom = Instantiate(roomPrefab, hitPose.position, hitPose.rotation, currentAnchor.transform);
                Debug.Log("Room instantiated and anchored at position: " + hitPose.position);

                // Adjust position to align the room with the ground
                AdjustRoomPosition();
            }
        }
    }

    private void AdjustRoomPosition()
    {
        if (spawnedRoom != null)
        {
            Renderer roomRenderer = spawnedRoom.GetComponentInChildren<Renderer>(); // Get the renderer from the prefab or its children
            if (roomRenderer != null)
            {
                Vector3 adjustedPosition = spawnedRoom.transform.position;
                adjustedPosition.y -= roomRenderer.bounds.min.y; // Align the bottom of the room with the ground
                spawnedRoom.transform.position = adjustedPosition;
                Debug.Log("Room adjusted to ground level: " + adjustedPosition);
            }
            else
            {
                Debug.LogWarning("Renderer not found in the room prefab.");
            }
        }
    }
}
