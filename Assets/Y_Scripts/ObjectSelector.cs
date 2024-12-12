using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ObjectSelector : MonoBehaviour
{
    public bool isCorrectTool = false; // Set to true for the urinary test model
    private Camera arCamera; // Reference to the AR Camera

    void Start()
    {
        // Assign the main AR camera
        arCamera = Camera.main;

        if (arCamera == null)
        {
            Debug.LogError("AR Camera not found. Make sure your AR setup is correct.");
        }
    }

    void Update()
    {
        // Handle touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Check if the touch just began
            if (touch.phase == TouchPhase.Began)
            {
                // Perform a raycast from the touch position
                Ray ray = arCamera.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    // Check if the object hit is this object
                    if (hit.transform == transform)
                    {
                        // Correct tool selected
                        if (isCorrectTool)
                        {
                            Debug.Log("Correct! You selected the urinary test model.");
                            GetComponent<Renderer>().material.color = Color.green;
                        }
                        // Incorrect tool selected
                        else
                        {
                            Debug.Log("Incorrect choice. Try again.");
                            GetComponent<Renderer>().material.color = Color.red;
                        }
                    }
                }
            }
        }
    }
}
