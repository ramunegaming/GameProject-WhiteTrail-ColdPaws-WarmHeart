using UnityEngine;

public class ScalingManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public float gridCellSize = 1f;    // Base grid cell size

    [Header("Player Scaling")]
    public float playerWidthInCells = 0.8f;     // Player should be slightly smaller than cell
    public float playerHeightInCells = 0.8f;    // How tall the player is relative to cell
    
    [Header("Vehicle Scaling")]
    public float carWidthInCells = 2f;          // Cars typically 2 cells wide
    public float carHeightInCells = 0.8f;       // Cars slightly shorter than cell height
    public float carLengthInCells = 1f;         // Cars take up 1 cell in depth
    
    [Header("References")]
    public GameObject player;
    public GameObject[] cars;
    public GameObject[] obstacles;

    void Start()
    {
        if (player != null)
        {
            ScalePlayer();
        }

        if (cars != null && cars.Length > 0)
        {
            ScaleCars();
        }

        if (obstacles != null && obstacles.Length > 0)
        {
            ScaleObstacles();
        }
    }

    void ScalePlayer()
    {
        Vector3 playerScale = new Vector3(
            gridCellSize * playerWidthInCells,
            gridCellSize * playerHeightInCells,
            gridCellSize * playerWidthInCells  // Using width for depth to keep it proportional
        );
        player.transform.localScale = playerScale;
    }

    void ScaleCars()
    {
        foreach (GameObject car in cars)
        {
            if (car != null)
            {
                Vector3 carScale = new Vector3(
                    gridCellSize * carWidthInCells,
                    gridCellSize * carHeightInCells,
                    gridCellSize * carLengthInCells
                );
                car.transform.localScale = carScale;
            }
        }
    }

    void ScaleObstacles()
    {
        foreach (GameObject obstacle in obstacles)
        {
            if (obstacle != null)
            {
                // Adjust scale based on the type of obstacle
                // This is an example - you might want to make this more specific
                Vector3 obstacleScale = new Vector3(
                    gridCellSize * 1f,  // Default to 1 cell width
                    gridCellSize * 1f,  // Default to 1 cell height
                    gridCellSize * 1f   // Default to 1 cell depth
                );
                obstacle.transform.localScale = obstacleScale;
            }
        }
    }

    // Utility method to scale any new objects added during gameplay
    public void ScaleObject(GameObject obj, float widthInCells, float heightInCells, float depthInCells)
    {
        Vector3 scale = new Vector3(
            gridCellSize * widthInCells,
            gridCellSize * heightInCells,
            gridCellSize * depthInCells
        );
        obj.transform.localScale = scale;
    }
}