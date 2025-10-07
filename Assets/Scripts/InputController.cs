using UnityEngine;

public class InputController : MonoBehaviour
{
    public GameManager gameManager;
    private PieceController selectedPiece;

    void Update()
    {
        // Mouse (for Editor)
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPos3D = gameManager.LogicTilemap.WorldToCell(world);
            Vector2Int cell = new Vector2Int(cellPos3D.x, cellPos3D.y);
            HandleClick(cell);
        }

        // Touch (for mobile)
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector3 world = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            Vector3Int cellPos3D = gameManager.LogicTilemap.WorldToCell(world);
            Vector2Int cell = new Vector2Int(cellPos3D.x, cellPos3D.y);
            HandleClick(cell);
        }
    }

    void HandleClick(Vector2Int cell)
    {
        Debug.Log($"Clicked cell: {cell}");

        // 🧩 Step 1: Check if you clicked on a piece
        foreach (Transform piece in gameManager.transform)
        {
            PieceController pc = piece.GetComponent<PieceController>();
            if (pc == null) continue;

            if (pc.currentCell == cell)
            {
                // Deselect if same piece
                if (selectedPiece == pc)
                {
                    DeselectPiece();
                    return;
                }

                // Select new piece
                SelectPiece(pc);
                return;
            }
        }

        // 🧩 Step 2: If a piece is selected, try to move it
        if (selectedPiece != null)
        {
            Debug.Log($"Trying to move from {selectedPiece.currentCell} to {cell}");
            selectedPiece.MoveTo(cell);
            DeselectPiece();
        }
    }

    void SelectPiece(PieceController piece)
    {
        DeselectPiece();
        selectedPiece = piece;
        piece.isSelected = true;

        var sr = piece.GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.color = Color.green; // Highlight selected
    }

    void DeselectPiece()
    {
        if (selectedPiece != null)
        {
            var sr = selectedPiece.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                // Restore correct team color
                if (selectedPiece.isKing)
                    sr.color = Color.yellow;
                else
                {
                    int id = selectedPiece.gameManager.board[selectedPiece.currentCell.x, selectedPiece.currentCell.y];
                    sr.color = id == 1 ? Color.red : Color.black;
                }
            }

            selectedPiece.isSelected = false;
            selectedPiece = null;
        }
    }
}
