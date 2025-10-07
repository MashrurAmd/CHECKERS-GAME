using UnityEngine;
using UnityEngine.Tilemaps;

public class DiceController : MonoBehaviour
{
    public Tilemap tilemap;
    public float moveSpeed = 5f; // Tiles per second
    public int boardSize = 8;    // Board is 8x8

    private bool isMoving = false;
    private Vector3 targetPosition;
    private Rigidbody2D rb;
    private bool isSelected = false;
    private Vector3Int lastPrintedCell;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void Update()
    {
        HandleMovement();
        HandleMouseInput();
    }

    void HandleMovement()
    {
        if (isMoving)
        {
            Vector3 newPos = Vector3.MoveTowards(rb.position, targetPosition, moveSpeed * Time.deltaTime);
            rb.MovePosition(newPos);

            // Print tile only when moving and tile changes
            Vector3Int currentCell = tilemap.WorldToCell(rb.position);
            if (currentCell != lastPrintedCell)
            {
                Debug.Log("Dice is at tile: (" + currentCell.x + ", " + currentCell.y + ")");
                lastPrintedCell = currentCell;
            }

            if (Vector3.Distance(rb.position, targetPosition) < 0.01f)
            {
                rb.position = targetPosition;
                isMoving = false;
            }
        }
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0;

            Collider2D clicked = Physics2D.OverlapPoint(mouseWorld);

            if (clicked == GetComponent<Collider2D>())
            {
                isSelected = true;
            }
            else if (isSelected)
            {
                Vector3Int targetCell = tilemap.WorldToCell(mouseWorld);

                // Clamp target to 8x8 board
                targetCell.x = Mathf.Clamp(targetCell.x, -boardSize / 2, boardSize / 2 - 1);
                targetCell.y = Mathf.Clamp(targetCell.y, -boardSize / 2, boardSize / 2 - 1);

                Vector3Int currentCell = tilemap.WorldToCell(transform.position);
                int dx = targetCell.x - currentCell.x;
                int dy = targetCell.y - currentCell.y;

                if (Mathf.Abs(dx) == Mathf.Abs(dy) && dx != 0)
                {
                    MoveToCell(targetCell);
                    isSelected = false;
                }
                else
                {
                    Debug.Log("Invalid move: only diagonal allowed!");
                }
            }
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
        rb.rotation = angle;
    }
}
