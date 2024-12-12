using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;  // Drag the TimerText UI element here in the Inspector
    [SerializeField] private float totalTime = 60f; // Set the initial countdown time in seconds
    [SerializeField] private GameObject playAgainButton; // Drag your "Play Again" button GameObject here

    private float currentTime;
    private bool isTimerRunning = false;

    void Start()
    {
        currentTime = totalTime;
        UpdateTimerUI();
        StartTimer();

        if (playAgainButton != null)
            playAgainButton.SetActive(false); // Ensure the button is hidden initially
    }

    void Update()
    {
        if (isTimerRunning)
        {
            currentTime -= Time.deltaTime; // Subtract elapsed time
            if (currentTime <= 0)
            {
                currentTime = 0;
                isTimerRunning = false;
                TimerEnded();
            }
            UpdateTimerUI();
        }
    }

    public void StartTimer()
    {
        isTimerRunning = true;
    }

    public void StopTimer()
    {
        isTimerRunning = false;
    }

    private void TimerEnded()
    {
        Debug.Log("Time's up!");
        EndGame(); // Call the EndGame function to stop gameplay and show the button
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            Debug.LogWarning("TimerText is not assigned in the Inspector.");
        }
    }

    private void EndGame()
    {
        // Show the Play Again button
        if (playAgainButton != null)
        {
            playAgainButton.SetActive(true);
        }

        // Unlock and show the mouse cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Stop all gameplay logic here
        Debug.Log("Gameplay has been disabled. Waiting for player to restart.");
    }
}


