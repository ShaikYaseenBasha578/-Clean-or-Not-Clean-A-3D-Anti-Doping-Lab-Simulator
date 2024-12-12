using UnityEngine;

public class ColliderSubtitle : MonoBehaviour
{
    public GameObject subtitleImage; // Assign your GUI Image (e.g., Panel or Image GameObject)
    public float displayDuration = 5f; // Time to show the subtitle

    private void Start()
    {
        // Ensure the subtitle image is initially hidden
        if (subtitleImage != null)
            subtitleImage.SetActive(false);
        else
            Debug.LogWarning("Subtitle Image is not assigned!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && subtitleImage != null) // Ensure only the player triggers the subtitle
        {
            ShowSubtitle();
        }
    }

    private void ShowSubtitle()
    {
        subtitleImage.SetActive(true); // Display the subtitle image
        Invoke("HideSubtitle", displayDuration); // Schedule hiding after the duration
    }

    private void HideSubtitle()
    {
        if (subtitleImage != null)
            subtitleImage.SetActive(false); // Hide the subtitle image
    }
}
