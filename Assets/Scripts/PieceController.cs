using UnityEngine;

public class PieceController : MonoBehaviour
{
    [HideInInspector] public GameManager gameManager;
    [HideInInspector] public Vector2Int currentCell;
    [HideInInspector] public bool isSelected = false;
    public bool isKing = false;

    void Start()
    {
        currentCell = gameManager.WorldToCell(transform.position);
    }

    public void MoveTo(Vector2Int targetCell)
    {
        if (!IsValidMove(targetCell, out Vector2Int? capturedCell)) return;

        int pieceId = gameManager.board[currentCell.x, currentCell.y];
        gameManager.board[currentCell.x, currentCell.y] = 0;

        // If we captured, remove opponent piece
        if (capturedCell.HasValue)
        {
            Vector2Int c = capturedCell.Value;
            gameManager.board[c.x, c.y] = 0;

            // Destroy the captured piece object
            foreach (Transform piece in gameManager.transform)
            {
                PieceController pc = piece.GetComponent<PieceController>();
                if (pc != null && pc.currentCell == c)
                {
                    Destroy(pc.gameObject);
                    break;
                }
            }
        }


        // Move piece
        gameManager.board[targetCell.x, targetCell.y] = pieceId;
        Vector3 worldPos = gameManager.LogicTilemap.GetCellCenterWorld(new Vector3Int(targetCell.x, targetCell.y, 0));
        transform.position = worldPos;

        currentCell = targetCell;
        isSelected = false;

        // King promotion
        if (pieceId == 1 && targetCell.y == 7) PromoteToKing();
        if (pieceId == -1 && targetCell.y == 0) PromoteToKing();


        
    }

    bool IsValidMove(Vector2Int target, out Vector2Int? capturedCell)
    {
        capturedCell = null;

        if (target.x < 0 || target.x >= 8 || target.y < 0 || target.y >= 8)
            return false;

        if (gameManager.board[target.x, target.y] != 0)
            return false;

        int pieceId = gameManager.board[currentCell.x, currentCell.y];
        int dir = pieceId; // 1 = red moves up, -1 = black moves down

        int dx = target.x - currentCell.x;
        int dy = target.y - currentCell.y;

        // NORMAL MOVE (1 step diagonal)
        if (Mathf.Abs(dx) == 1 && (dy == dir || (isKing && Mathf.Abs(dy) == 1)))
            return true;

        // CAPTURE MOVE (2 steps diagonal)
        if (Mathf.Abs(dx) == 2 && (dy == 2 * dir || (isKing && Mathf.Abs(dy) == 2)))
        {
            Vector2Int mid = new Vector2Int(currentCell.x + dx / 2, currentCell.y + dy / 2);
            int midPiece = gameManager.board[mid.x, mid.y];

            // Must be an opponent piece
            if (midPiece != 0 && Mathf.Sign(midPiece) != Mathf.Sign(pieceId))
            {
                capturedCell = mid;
                return true;
            }
        }

        return false;
    }

    void PromoteToKing()
    {
        isKing = true;
        GetComponent<SpriteRenderer>().color = Color.yellow; // visually mark as king
    }
}
