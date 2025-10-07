using UnityEngine;
using UnityEngine.Tilemaps;

public class DiceController : MonoBehaviour
{
    public Tilemap tilemap; // Assign your Tilemap in the inspector
    public float moveSpeed = 5f; // Speed of movement
    private bool isMoving = false;
    private Vector3 targetPosition;

    void Update()
    {
        if (isMoving)
        {
            // Move towards target
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;
                isMoving = false;
            }
        }
    }

    void OnMouseDown()
    {
        // Detect click and show options (diagonals)
        // For simplicity, we'll use arrow keys for now, can replace with UI buttons
        Debug.Log("Dice clicked! Press arrow keys to move diagonally.");
    }

    void FixedUpdate()
    {
        if (!isMoving)
        {
            Vector3Int currentCell = tilemap.WorldToCell(transform.position);

            // Diagonal movement via arrow keys (for testing)
            if (Input.GetKeyDown(KeyCode.W) && Input.GetKeyDown(KeyCode.D))
                MoveToCell(currentCell + new Vector3Int(1, 1, 0));
            else if (Input.GetKeyDown(KeyCode.W) && Input.GetKeyDown(KeyCode.A))
                MoveToCell(currentCell + new Vector3Int(-1, 1, 0));
            else if (Input.GetKeyDown(KeyCode.S) && Input.GetKeyDown(KeyCode.D))
                MoveToCell(currentCell + new Vector3Int(1, -1, 0));
            else if (Input.GetKeyDown(KeyCode.S) && Input.GetKeyDown(KeyCode.A))
                MoveToCell(currentCell + new Vector3Int(-1, -1, 0));
        }
    }

    public void MoveToCell(Vector3Int cellPosition)
    {
        targetPosition = tilemap.GetCellCenterWorld(cellPosition);
        RotateToTarget(targetPosition);
        isMoving = true;
    }

    void RotateToTarget(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
