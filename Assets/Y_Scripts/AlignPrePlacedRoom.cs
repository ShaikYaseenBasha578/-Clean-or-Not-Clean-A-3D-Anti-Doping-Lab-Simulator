using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class AlignPrePlacedRoom : MonoBehaviour
{
    private ARPlaneManager planeManager;
    private ARAnchorManager anchorManager; // For creating anchors

    public GameObject roomPrefab; // Reference to the pre-placed room
    private ARAnchor roomAnchor; // Anchor for the room

    void Start()
    {
        planeManager = FindObjectOfType<ARPlaneManager>();
        anchorManager = FindObjectOfType<ARAnchorManager>();

        if (planeManager == null)
        {
            Debug.LogError("ARPlaneManager not found in the scene.");
        }

        if (anchorManager == null)
        {
            Debug.LogError("ARAnchorManager not found in the scene.");
        }
    }

    [System.Obsolete]
    void Update()
    {
        // Align the room with the first detected plane
        foreach (var plane in planeManager.trackables)
        {
            if (plane.alignment == PlaneAlignment.HorizontalUp)
            {
                AlignRoomToPlane(plane);
                DisablePlaneDetection(); // Optional: Disable plane detection after alignment
                break;
            }
        }
    }

    [System.Obsolete]
    private void AlignRoomToPlane(ARPlane plane)
    {
        if (roomPrefab != null && plane != null)
        {
            // Destroy the previous anchor if one exists
            if (roomAnchor != null)
            {
                Destroy(roomAnchor.gameObject);
            }

            // Create an anchor at the plane's position
            Pose planePose = new Pose(plane.transform.position, plane.transform.rotation);
            roomAnchor = anchorManager.AddAnchor(planePose);

            if (roomAnchor != null)
            {
                // Parent the room to the anchor for stability
                roomPrefab.transform.SetParent(roomAnchor.transform, true);
                roomPrefab.transform.localPosition = Vector3.zero; // Align with the anchor

                Debug.Log("Room anchored to the detected plane.");
            }
            else
            {
                Debug.LogError("Failed to create anchor for the room.");
            }
        }
    }

    private void DisablePlaneDetection()
    {
        // Disable plane visuals and detection for a cleaner environment
        foreach (var plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }

        planeManager.enabled = false;
    }
}
