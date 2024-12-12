using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TestResultManager : MonoBehaviour
{
    public TMP_Text resultText; // UI Text to display the result
    public Canvas resultCanvas; // Canvas to show/hide results

    private string[] possibleResults = { "Negative", "Positive", "Inconclusive" }; // Example results

    void Start()
    {
        resultCanvas.gameObject.SetActive(false); // Ensure the canvas is hidden at the start
    }

    public void ShowResult()
    {
        string randomResult = possibleResults[Random.Range(0, possibleResults.Length)];
        Debug.Log("Test Result: " + randomResult); // Log to ensure this is being called

        resultText.text = ""; // Clear existing text first (optional)
        resultText.text = "Test Result: " + randomResult; // Update with new result
        resultCanvas.gameObject.SetActive(true); // Show result Canvas
    }


    public void CloseResult()
    {
        resultCanvas.gameObject.SetActive(false); // Hide the canvas
    }
}
