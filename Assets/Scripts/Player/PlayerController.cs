using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Hop Movement Settings")]
    public float hopDistance = 1f;
    public float hopHeight = 0.5f;
    public float hopDuration = 0.3f;
    public float rotationDuration = 0.1f;

    [Header("Safety Detection")]
    public float safetyCheckDistance = 0.5f;
    public LayerMask safeGroundLayer;
    public LayerMask dangerousLayer;
    public LayerMask fallLayer;

    [Header("Croak Ability")]
    public float croakRadius = 10f;
    public float croakCooldown = 2f;

    private bool isHopping = false;
    private float lastCroakTime;

    private HUDManager hudManager;
    private GameManager gameManager;

    void Awake()
    {
        hudManager = Object.FindAnyObjectByType<HUDManager>();
        gameManager = Object.FindAnyObjectByType<GameManager>();
    }

    void Update()
    {
        if (!isHopping)
        {
            HandleMovementInput();
            HandleCroak();
        }
    }

    void HandleMovementInput()
    {
        Vector3 moveDirection = Vector3.zero;

        if (Input.GetKeyDown(KeyCode.W)) moveDirection = Vector3.forward;
        else if (Input.GetKeyDown(KeyCode.S)) moveDirection = Vector3.back;
        else if (Input.GetKeyDown(KeyCode.A)) moveDirection = Vector3.left;
        else if (Input.GetKeyDown(KeyCode.D)) moveDirection = Vector3.right;

        if (moveDirection != Vector3.zero)
        {
            // Convert local direction to world space
            StartCoroutine(PerformHopWithRotation(moveDirection));
        }
    }

    IEnumerator PerformHopWithRotation(Vector3 direction)
    {
        isHopping = true;

        // Rotation Phase
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

        // Rotate only if not already facing the direction
        if (Quaternion.Angle(transform.rotation, targetRotation) > 1f)
        {
            float elapsedRotationTime = 0f;
            Quaternion startRotation = transform.rotation;

            while (elapsedRotationTime < rotationDuration)
            {
                transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedRotationTime / rotationDuration);
                elapsedRotationTime += Time.deltaTime;
                yield return null;
            }

            // Ensure exact rotation
            transform.rotation = targetRotation;
        }

        // Hop Phase
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + direction * hopDistance;

        // Hop Animation
        float elapsedTime = 0f;
        while (elapsedTime < hopDuration)
        {
            float verticalOffset = Mathf.Sin(elapsedTime / hopDuration * Mathf.PI) * hopHeight;

            Vector3 currentPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / hopDuration);
            transform.position = new Vector3(
                currentPosition.x, 
                startPosition.y + verticalOffset, 
                currentPosition.z
            );

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final position is exact
        transform.position = targetPosition;
        isHopping = false;
    }

    void HandleCroak()
    {
        if (Input.GetKeyDown(KeyCode.E) && Time.time >= lastCroakTime + croakCooldown)
        {
            PerformCroak();
            lastCroakTime = Time.time;
        }
    }

    void PerformCroak()
    {
        Collider[] nearbyCollectibles = Physics.OverlapSphere(transform.position, croakRadius);
        
        bool collectibleFound = false;
        foreach (Collider collectible in nearbyCollectibles)
        {
            if (collectible.CompareTag("CollectibleFrog"))
            {
                HighlightCollectible(collectible.gameObject);
                collectibleFound = true;
            }
        }

        // Provide feedback
        if (collectibleFound)
        {
            Debug.Log("Collectible Frog located!");
        }
        else
        {
            Debug.Log("No Collectible Frogs nearby.");
        }
    }

    void HighlightCollectible(GameObject collectible)
    {
        Renderer renderer = collectible.GetComponent<Renderer>();
        if (renderer != null)
        {
            StartCoroutine(HighlightTemporarily(renderer));
        }
    }

    IEnumerator HighlightTemporarily(Renderer renderer)
    {
        Color originalColor = renderer.material.color;
        renderer.material.color = Color.yellow;
        yield return new WaitForSeconds(1f);
        renderer.material.color = originalColor;
    }

    // Trigger detection for player and car collision
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            // Call GameOver method if player hits a car
            gameManager?.GameOver();
        }
    }

    bool CheckSafetyAtPosition(Vector3 position)
    {
        return Physics.Raycast(
            position + Vector3.up * 0.1f, 
            Vector3.down, 
            safetyCheckDistance, 
            safeGroundLayer
        );
    }

    bool IsOverFallArea(Vector3 position)
    {
        return Physics.Raycast(
            position + Vector3.up * 0.1f, 
            Vector3.down, 
            safetyCheckDistance, 
            fallLayer
        );
    }

    bool IsOverDangerousArea(Vector3 position)
    {
        return Physics.Raycast(
            position + Vector3.up * 0.1f, 
            Vector3.down, 
            safetyCheckDistance, 
            dangerousLayer
        );
    }

    IEnumerator PerformFall()
    {
        Vector3 fallStartPosition = transform.position;
        float fallDuration = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < fallDuration)
        {
            transform.position = Vector3.Lerp(
                fallStartPosition, 
                fallStartPosition + Vector3.down * 10f, 
                elapsedTime / fallDuration
            );
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        HandlePlayerDeath();
    }

    void HandlePlayerDeath()
    {
        gameManager?.GameOver();
    }
}