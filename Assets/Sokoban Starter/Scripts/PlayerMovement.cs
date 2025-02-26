using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveDelay = 0.2f; // Prevents rapid movement

    [Header("Grid Settings")]
    public Vector2Int gridSize = new Vector2Int(10, 5); // Editable grid size
    public Vector2Int startGridPosition = new Vector2Int(0, 4); // Editable start position

    private Vector2Int position; // Stores the player’s grid position
    private float lastMoveTime;

    private void Start()
    {
        // Ensure the player starts within the grid boundaries
        position = new Vector2Int(
            Mathf.Clamp(startGridPosition.x, 0, gridSize.x - 1),
            Mathf.Clamp(startGridPosition.y, 0, gridSize.y - 1)
        );

        transform.position = GridToWorldPosition(position);

        // Register the player in the GridManager
        GridManager.Instance.RegisterBlock(position, gameObject);
    }

    private void Update()
    {
        if (Time.time - lastMoveTime < moveDelay) return;

        Vector2Int moveDirection = Vector2Int.zero;

        if (Input.GetKeyDown(KeyCode.W)) moveDirection = Vector2Int.up;
        else if (Input.GetKeyDown(KeyCode.S)) moveDirection = Vector2Int.down;
        else if (Input.GetKeyDown(KeyCode.A)) moveDirection = Vector2Int.left;
        else if (Input.GetKeyDown(KeyCode.D)) moveDirection = Vector2Int.right;

        if (moveDirection != Vector2Int.zero)
            TryMove(moveDirection);
    }

    private void TryMove(Vector2Int direction)
    {
        Vector2Int newPosition = position + direction;

        if (!IsWithinBounds(newPosition))
            return; // Prevent out-of-bounds movement

        if (GridManager.Instance.IsCellOccupied(newPosition))
            return; // Prevent movement into occupied cells

        // Move the player
        GridManager.Instance.UnregisterBlock(position);
        position = newPosition;
        transform.position = GridToWorldPosition(position);
        GridManager.Instance.RegisterBlock(position, gameObject);

        lastMoveTime = Time.time;
    }

    private bool IsWithinBounds(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < gridSize.x && pos.y >= 0 && pos.y < gridSize.y;
    }

    // Converts grid coordinates to world position
    private Vector3 GridToWorldPosition(Vector2Int gridPos)
    {
        return new Vector3(gridPos.x - (gridSize.x / 2f) + 0.5f, gridPos.y - (gridSize.y / 2f) + 0.5f, 0);
    }
}
