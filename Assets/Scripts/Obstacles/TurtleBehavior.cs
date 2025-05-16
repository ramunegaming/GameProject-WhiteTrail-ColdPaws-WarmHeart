using UnityEngine;

public class TurtleBehavior : MonoBehaviour
{
    public enum MovementDirection
    {
        PositiveX,
        NegativeX,
        PositiveY,
        NegativeY,
        PositiveZ,
        NegativeZ
    }

    [Header("Movement Settings")]
    public MovementDirection direction = MovementDirection.NegativeX;
    public float moveSpeed = 2f;
    
    [Header("Wrapping Settings")]
    public float xStartPosition = 10f;
    public float xEndPosition = -10f;
    public float zStartPosition = 10f;
    public float zEndPosition = -10f;

    [Header("Diving Settings")]
    public float minTimeBetweenDives = 5f;
    public float maxTimeBetweenDives = 15f;
    public float waterLevel = 0f;
    public float diveDepth = -1f;
    public float diveDuration = 3f;
    public float diveSpeed = 2f;

    private float nextDiveTime;
    private bool isUnderwater = false;
    private Vector3 originalPosition;
    private float diveTimer = 0f;

    private void Start()
    {
        originalPosition = transform.position;
        SetNextDiveTime();
    }

    private void Update()
    {
        Vector3 movement = GetMovementVector();
        transform.Translate(movement * moveSpeed * Time.deltaTime);

        HandleScreenWrap();

        if (Time.time >= nextDiveTime && !isUnderwater)
        {
            StartDiving();
        }
        else if (isUnderwater)
        {
            HandleUnderwaterState();
        }
    }

    private Vector3 GetMovementVector()
    {
        return direction switch
        {
            MovementDirection.PositiveX => Vector3.right,
            MovementDirection.NegativeX => Vector3.left,
            MovementDirection.PositiveY => Vector3.up,
            MovementDirection.NegativeY => Vector3.down,
            MovementDirection.PositiveZ => Vector3.forward,
            MovementDirection.NegativeZ => Vector3.back,
            _ => Vector3.zero
        };
    }

    private void HandleScreenWrap()
    {
        Vector3 pos = transform.position;

        if (direction == MovementDirection.NegativeZ && pos.z <= zEndPosition)
        {
            transform.position = new Vector3(pos.x, pos.y, zStartPosition);
        }
        else if (direction == MovementDirection.PositiveZ && pos.z >= zStartPosition)
        {
            transform.position = new Vector3(pos.x, pos.y, zEndPosition);
        }
        else if (direction == MovementDirection.NegativeX && pos.x <= xEndPosition)
        {
            transform.position = new Vector3(xStartPosition, pos.y, pos.z);
        }
        else if (direction == MovementDirection.PositiveX && pos.x >= xStartPosition)
        {
            transform.position = new Vector3(xEndPosition, pos.y, pos.z);
        }
    }

    private void StartDiving()
    {
        isUnderwater = true;
        diveTimer = 0f;
    }

    private void HandleUnderwaterState()
    {
        diveTimer += Time.deltaTime;
        
        if (diveTimer <= diveSpeed)
        {
            float newY = Mathf.Lerp(originalPosition.y, waterLevel + diveDepth, diveTimer / diveSpeed);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
        else if (diveTimer >= diveDuration - diveSpeed)
        {
            float riseProgress = (diveTimer - (diveDuration - diveSpeed)) / diveSpeed;
            float newY = Mathf.Lerp(waterLevel + diveDepth, originalPosition.y, riseProgress);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            
            if (diveTimer >= diveDuration)
            {
                isUnderwater = false;
                SetNextDiveTime();
            }
        }
    }

    private void SetNextDiveTime()
    {
        nextDiveTime = Time.time + Random.Range(minTimeBetweenDives, maxTimeBetweenDives);
    }

    public bool CanSupportFrog()
    {
        return !isUnderwater;
    }
}