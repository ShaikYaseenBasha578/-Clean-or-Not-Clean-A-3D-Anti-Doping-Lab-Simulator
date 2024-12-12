using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseChest : MonoBehaviour
{
    public GameObject handUI;
    public GameObject objToActivate;
    public GameObject objToDeactivate;

    private bool inReach = false;
    private bool isAnalyzing = false;
    private bool canInteract = false;

    // Reference to TestResultManager
    public TestResultManager testResultManager;

    void Start()
    {
        handUI.SetActive(false);

        if (objToActivate != null)
            objToActivate.SetActive(false);

        if (objToDeactivate != null)
            objToDeactivate.SetActive(false); // Start with Empty Arms deactivated
    }

    void OnTriggerEnter(Collider other)
    {
        // Check for Reach trigger and prevent multiple interactions at the same time
        if (other.gameObject.CompareTag("Reach") && !isAnalyzing)
        {
            inReach = true;
            canInteract = true;  // Player can interact with chest
            handUI.SetActive(true);
        }
        // Check for Analyzer trigger
        else if (other.gameObject.CompareTag("Analyzer"))
        {
            isAnalyzing = true;
            canInteract = true;  // Player can interact with analyzer
            handUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if player leaves Reach trigger
        if (other.gameObject.CompareTag("Reach") && !isAnalyzing)
        {
            inReach = false;
            canInteract = false;  // No longer able to interact with chest
            handUI.SetActive(false);
        }
        // Check if player leaves Analyzer trigger
        else if (other.gameObject.CompareTag("Analyzer"))
        {
            isAnalyzing = false;
            canInteract = false;  // No longer able to interact with analyzer
            handUI.SetActive(false);
        }
    }

    void Update()
    {
        // Check if player presses the "E" key and is in range to interact
        if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    // This method is called when the player presses the interact button
    public void Interact()
    {
        if (canInteract)
        {
            if (inReach)
            {
                Debug.Log("Interacting with Chest.");
                handUI.SetActive(false);

                if (objToActivate != null)
                {
                    objToActivate.SetActive(true); // Activate the object (e.g., Sample Arms)
                    Debug.Log($"{objToActivate.name} activated.");
                }

                if (objToDeactivate != null)
                {
                    objToDeactivate.SetActive(false); // Deactivate the object (e.g., Empty Arms)
                    Debug.Log($"{objToDeactivate.name} deactivated.");
                }
            }
            else if (isAnalyzing)
            {
                Debug.Log("Interacting with Analyzer.");
                handUI.SetActive(false);

                if (objToActivate != null)
                {
                    objToActivate.SetActive(false); // Deactivate object (e.g., Sample Arms)
                    Debug.Log($"{objToActivate.name} deactivated.");
                }

                if (objToDeactivate != null)
                {
                    objToDeactivate.SetActive(true); // Reactivate object (e.g., Empty Arms)
                    Debug.Log($"{objToDeactivate.name} activated.");
                }

                // Call the ShowResult method from the TestResultManager to display the result
                if (testResultManager != null)
                {
                    testResultManager.ShowResult(); // Show result when interacting with the analyzer
                }
                else
                {
                    Debug.LogWarning("TestResultManager is not assigned!");
                }
            }
        }
    }
}

