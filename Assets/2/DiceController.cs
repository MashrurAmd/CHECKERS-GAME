using UnityEngine;
using UnityEngine.Tilemaps;

public class DiceController : MonoBehaviour
{
    public Tilemap tilemap;         // assign your tilemap
    public float moveSpeed = 5f;    // tiles per second

    private bool isMoving = false;
    private Vector3 targetPosition;
    private Rigidbody2D rb;
    private bool isSelected = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic; // ensure it doesn't fall
    }

    void Update()
    {
        // Handle movement
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

        // Deselect if clicked elsewhere
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0;

            // If clicking on the dice
            if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(mouseWorld))
            {
                isSelected = true;
            }
            else if (isSelected)
            {
                // Move to clicked tile
                Vector3Int targetCell = tilemap.WorldToCell(mouseWorld);

                // Ensure diagonal movement (x and y both change)
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
                    Debug.Log("You can only move diagonally!");
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
