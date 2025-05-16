using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform player;               // Reference to the player model

    [Header("Camera Positioning")]
    public float verticalHeight = 10f;     // Height above the player
    public float backwardOffset = 7f;      // Distance behind the player
    public float sideOffset = 0f;          // Optional side offset

    [Header("Camera Angle")]
    public Vector3 fixedRotation = new Vector3(45f, 0f, 0f); // Fixed camera angle

    [Header("Smoothing")]
    public float smoothSpeed = 5f;         // Smoothness of camera movement

    void LateUpdate()
    {
        if (player == null)
        {
            Debug.LogWarning("Player Transform is not assigned.");
            return;
        }

        // Calculate the desired position for the camera
        Vector3 desiredPosition = player.position +
                                  (Vector3.up * verticalHeight) +         // Height above the player
                                  (-Vector3.forward * backwardOffset) +   // Fixed distance behind the player
                                  (Vector3.right * sideOffset);           // Optional side offset

        // Smoothly interpolate the camera's position
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );

        // Keep the camera at a fixed rotation
        transform.rotation = Quaternion.Euler(fixedRotation);
    }
}
