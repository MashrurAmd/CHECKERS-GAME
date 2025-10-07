using UnityEngine;
using UnityEngine.Tilemaps;

public class DiceController : MonoBehaviour
{
    public Tilemap tilemap;          // Assign Tilemap component here
    public float moveSpeed = 5f;     // Tiles per second

    private bool isMoving = false;
    private Vector3 targetPosition;
    private Rigidbody2D rb;
    private bool isSelected = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic; // Prevent falling
    }

    void Update()
    {
        HandleMovement();
        HandleMouseInput();

        // Print dice's current tile position without z
        Vector3Int currentCell = tilemap.WorldToCell(transform.position);
        Debug.Log("Dice is at tile: (" + currentCell.x + ", " + currentCell.y + ")");
    }

    void HandleMovement()
    {
        if (isMoving)
        {
            Vector3 newPos = Vector3.MoveTowards(rb.position, targetPosition, moveSpeed * Time.deltaTime);
            rb.MovePosition(newPos);

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
                // Dice clicked → select it
                isSelected = true;
            }
            else if (isSelected)
            {
                // Mouse clicked somewhere else → attempt to move
                Vector3Int targetCell = tilemap.WorldToCell(mouseWorld);
                Vector3Int currentCell = tilemap.WorldToCell(transform.position);

                int dx = targetCell.x - currentCell.x;
                int dy = targetCell.y - currentCell.y;

                if (Mathf.Abs(dx) == Mathf.Abs(dy) && dx != 0) // must be diagonal
                {
                    MoveToCell(targetCell);
                    isSelected = false; // optional: deselect after move
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
