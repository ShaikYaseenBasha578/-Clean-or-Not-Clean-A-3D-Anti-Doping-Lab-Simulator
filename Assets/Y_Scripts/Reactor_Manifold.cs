using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ReactorManifold : MonoBehaviour
{
    [SerializeField] private int nextButton; // Tracks the next button in the sequence
    [SerializeField] private GameObject GamePanel; // The panel for the mini-game
    [SerializeField] private Button[] myButtons; // Buttons in the mini-game

    private List<int> numbers = new List<int>(); // List to hold the numbers for randomization

    private void Start()
    {
        nextButton = 0; // Start with the first button
    }

    private void OnEnable() // Called when the mini-game is activated
    {
        ResetGame();
    }

    private void ResetGame()
    {
        nextButton = 0; // Reset the sequence
        numbers.Clear();

        // Fill the numbers list with values from 0 to the number of buttons
        for (int i = 0; i < myButtons.Length; i++)
        {
            numbers.Add(i);
        }

        // Shuffle and assign numbers to button texts
        foreach (Button button in myButtons)
        {
            int randomIndex = Random.Range(0, numbers.Count);
            int assignedNumber = numbers[randomIndex];
            numbers.RemoveAt(randomIndex);

            // Update the button text (TextMeshPro version)
            TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                buttonText.text = assignedNumber.ToString();
            }

            // Assign the correct number to the button click event
            int capturedIndex = assignedNumber; // Capture the number for the lambda
            button.onClick.RemoveAllListeners(); // Clear previous listeners
            button.onClick.AddListener(() => ButtonOrder(capturedIndex));
        }
    }

    public void ButtonOrder(int button)
    {
        Debug.Log("Pressed");

        if (button == nextButton)
        {
            nextButton++;
            Debug.Log("Next Button: " + nextButton);

            if (nextButton == myButtons.Length)
            {
                Debug.Log("Mini-game completed!");
                ButtonOrderPanelClose();
            }
        }
        else
        {
            Debug.Log("Incorrect button! Restarting...");
            ResetGame();
        }
    }

    public void ButtonOrderPanelClose() // Closes the mini-game panel
    {
        GamePanel.SetActive(false);
        UnlockCursor();
    }

    public void ButtonOrderPanelOpen() // Opens the mini-game panel
    {
        GamePanel.SetActive(true);
        UnlockCursor();
        ResetGame();
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true; // Make it visible
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
        Cursor.visible = false; // Hide the cursor
    }
}
