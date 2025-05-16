using UnityEngine;

public class CarBehavior : MonoBehaviour
{
    public enum CarType
    {
        Car1,
        Car2,
        BoxTruck,
        Bulldozer,
        Lorry
    }

    [Header("Car Settings")]
    public CarType carType;       // Type of the car (selectable in the Inspector)
    public Vector3 startPosition; // Starting position of the car
    public Vector3 endPosition;   // Ending position of the car
    public Vector3 direction;     // Movement direction (normalized)
    public float speed = 5f;      // Speed of the car

    void Start()
    {
        // Set the car's initial position to the start position
        transform.position = startPosition;
    }

    void Update()
    {
        // Move the car in the specified direction at the given speed
        MoveCar();
    }

    private void MoveCar()
    {
        // Move the car towards the end position
        transform.position = Vector3.MoveTowards(transform.position, endPosition, speed * Time.deltaTime);

        // Check if the car has reached the end position
        if (Vector3.Distance(transform.position, endPosition) < 0.1f)
        {
            ResetCar();
        }
    }

    private void ResetCar()
    {
        // Reset the car to the start position
        transform.position = startPosition;
    }

    // Optional: Debug to visualize the path in the Scene view
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(startPosition, 0.2f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(endPosition, 0.2f);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(startPosition, endPosition);
    }
}