using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RespawnPlayer()
    {
        // Reload current scene or move to a checkpoint
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameOver()
    {
        Debug.Log("Game Over!");
        // Additional game over logic
        SceneManager.LoadScene("MainMenu"); // Ensure you have a MainMenu scene
    }

    // New method to handle player death
    public void HandlePlayerDeath()
    {
        // Game over logic (you can show a UI, play a sound, etc.)
        Debug.Log("Player has been hit by a car. Game Over.");
        GameOver(); // Call the GameOver method
    }
}