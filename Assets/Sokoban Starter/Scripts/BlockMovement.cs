using UnityEngine;

public class BlockMovement : MonoBehaviour
{
    private Vector2Int position;

    private void Start()
    {
        position = Vector2Int.RoundToInt(transform.position);
        GridManager.Instance.RegisterBlock(position, gameObject);
    }

    public bool TryMove(Vector2Int direction)
    {
        Vector2Int newPosition = position + direction;

        if (!GridManager.Instance.IsWithinBounds(newPosition))
            return false;

        if (GridManager.Instance.IsCellOccupied(newPosition))
            return false;

        // Move the block
        GridManager.Instance.UnregisterBlock(position);
        position = newPosition;
        transform.position = new Vector3(position.x, position.y, 0);
        GridManager.Instance.RegisterBlock(position, gameObject);

        return true;
    }
}
