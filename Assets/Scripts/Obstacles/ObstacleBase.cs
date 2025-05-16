using UnityEngine;

public abstract class ObstacleBase : MonoBehaviour
{
    public float damage = 1f;
    public float speed = 5f;

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                HandlePlayerCollision(player);
            }
        }
    }

    protected virtual void HandlePlayerCollision(PlayerController player)
    {
        // Default collision behavior
        Debug.Log("Player hit an obstacle!");
        
        // Potentially reduce player health or trigger game over
        // You might want to call a method in GameManager or HUDManager
    }

    public abstract void Move();
}