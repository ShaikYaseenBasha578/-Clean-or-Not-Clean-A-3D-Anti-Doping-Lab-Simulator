using UnityEngine;
using TMPro;  // Include TextMeshPro namespace

public class ReadNotes : MonoBehaviour
{
    public GameObject player;
    public GameObject noteUI;
    public GameObject hud;
    public GameObject inv;

    public GameObject pickUpText;

    // UI Elements for Feedback
    public TMP_Text resultText;  // TextMeshPro component to display substance amounts
    public TMP_Text feedbackText;  // TextMeshPro component for Correct/Incorrect feedback
    public TMP_Text scoreText;  // TextMeshPro component for displaying score
    public GameObject feedbackPanel;  // Panel to show feedback (correct/incorrect)

    // Substances and their values
    private float[] substances = new float[4]; // Hold substance values
    private string[] substanceNames = { "Testosterone", "Erythropoietin (EPO)", "Human Growth Hormone (HGH)", "Clenbuterol" }; // Substance names

    // Score and feedback colors
    private int score = 0;
    public Color correctColor = Color.green; // Green for Correct
    public Color incorrectColor = Color.red; // Red for Incorrect

    private bool inReach;
    private bool canInteract;

    void Start()
    {
        noteUI.SetActive(false);
        hud.SetActive(true);
        inv.SetActive(true);
        pickUpText.SetActive(false);
        feedbackPanel.SetActive(false);

        inReach = false;
        canInteract = false;  // Initially not interactable
        GenerateSubstanceValues();
        UpdateUI();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Reach"))
        {
            inReach = true;
            pickUpText.SetActive(true);
            canInteract = true;  // Player is in reach, can interact
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Reach"))
        {
            inReach = false;
            pickUpText.SetActive(false);
            canInteract = false;  // Player is out of reach, stop interaction
        }
    }


    void Update()
    {
        if (Input.GetButtonDown("Interact") && inReach)
        {
            if (this.gameObject.GetComponent<ReadNotes>() != null)
            {
                Debug.Log("Interacting with: " + this.gameObject.name);
                InteractButtonClicked(); // Call the interaction logic
            }
        }
    }



    // This method is called when the Interact button is pressed
    public void InteractButtonClicked()
    {
        noteUI.SetActive(true);
        hud.SetActive(false);
        inv.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        feedbackPanel.SetActive(false); // Hide the previous feedback panel
        feedbackText.text = ""; // Clear the feedback text

        GenerateSubstanceValues(); // Generate new values for the sample
        UpdateUI(); // Update the UI with the new values
    }

    // Only allow interaction if the player is in reach and can interact
    public void OnInteractButton()
    {
        if (canInteract)
        {
            noteUI.SetActive(true);
            hud.SetActive(false);
            inv.SetActive(false);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            feedbackPanel.SetActive(false);
            feedbackText.text = "";

            GenerateSubstanceValues();
            UpdateUI();
        }
    }


    // Called when the player chooses "Clean"
    public void IsCleanButtonClicked()
    {
        CheckCleanliness(true); // True indicates clean
    }

    // Called when the player chooses "Not Clean"
    public void IsNotCleanButtonClicked()
    {
        CheckCleanliness(false); // False indicates unclean
    }

    // Method to check if the sample is clean or not based on the substances
    void CheckCleanliness(bool isClean)
    {
        bool isCorrect = IsSampleClean(isClean);  // Check if the player's decision is correct

        // Provide feedback and update score
        if (isCorrect)
        {
            feedbackText.text = "Correct!";
            feedbackPanel.GetComponent<UnityEngine.UI.Image>().color = correctColor;  // Change feedback panel color to green
            score++;  // Increase score for correct answer
        }
        else
        {
            feedbackText.text = "Incorrect!";
            feedbackPanel.GetComponent<UnityEngine.UI.Image>().color = incorrectColor;  // Change feedback panel color to red
        }

        feedbackPanel.SetActive(true);  // Show feedback panel
        UpdateScoreUI();  // Update the score UI
    }

    // Method to check if the sample is clean or not based on substances
    bool IsSampleClean(bool isClean)
    {
        float threshold = 0.5f;  // Example threshold for cleanliness
        bool isCleanSample = true;

        foreach (float substance in substances)
        {
            if (substance > threshold)
            {
                isCleanSample = false;  // If any substance exceeds the threshold, it's considered unclean
                break;
            }
        }

        return isClean == isCleanSample;  // Return true if the player's guess matches the actual result
    }

    // Method to update the score UI
    void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score.ToString();  // Display the updated score
    }

    // Method to generate random values for substances within realistic ranges
    void GenerateSubstanceValues()
    {
        substances[0] = Random.Range(0.3f, 1.0f); // Testosterone (0.3–1.0 ng/mL)
        substances[1] = Random.Range(0f, 10f);   // EPO (0–10 mIU/mL)
        substances[2] = Random.Range(0.01f, 0.5f); // HGH (0.01–0.5 ng/mL)
        substances[3] = Random.Range(0f, 1f);    // Clenbuterol (<1 ng/mL)
    }

    // Method to update the UI text with the substance values
    void UpdateUI()
    {
        resultText.text = "";  // Reset the result text
        for (int i = 0; i < substances.Length; i++)
        {
            resultText.text += substanceNames[i] + ": " + substances[i].ToString("F2") + " ng/mL\n";  // Display the values
        }
    }

    // Exit Button functionality to close the notes
    public void ExitButton()
    {
        noteUI.SetActive(false);
        hud.SetActive(true);
        inv.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}

