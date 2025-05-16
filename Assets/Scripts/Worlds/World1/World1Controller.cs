using UnityEngine;
using UnityEngine.SceneManagement; // For loading scenes

public class World1Controller : MonoBehaviour
{
    // Reference to the GameManager for handling game over state
    private GameManager gameManager;

    void Awake()
    {
        // Get the GameManager component from the scene
        gameManager = Object.FindAnyObjectByType<GameManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the player collided with a car
        if (other.CompareTag("Car"))
        {
            // Trigger the GameOver method in GameManager
            gameManager?.GameOver();
        }
    }
}