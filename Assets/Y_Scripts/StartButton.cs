using UnityEngine;
using UnityEngine.SceneManagement;  // Required for scene management

public class StartButton : MonoBehaviour
{
    // Function to start the game
    public void StartGame()
    {
        // Load the first scene or the game scene (replace "GameScene" with your scene name)
        SceneManager.LoadScene("SIH_3D");
    }
}

