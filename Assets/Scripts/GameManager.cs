using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    [Header("Board")]
    public Tilemap LogicTilemap; // Assign your Tilemap in Inspector
    public int[,] board = new int[8, 8];

    [Header("Prefabs")]
    public GameObject redPiecePrefab;
    public GameObject blackPiecePrefab;

    void Start()
    {
        InitializeBoard();
        SpawnInitialPieces();
    }

    void InitializeBoard()
    {
        for (int x = 0; x < 8; x++)
            for (int y = 0; y < 8; y++)
                board[x, y] = 0;
    }

    void SpawnInitialPieces()
    {
        // Black pieces
        for (int y = 0; y < 3; y++)
            for (int x = 0; x < 8; x++)
                if ((x + y) % 2 == 1)
                    SpawnPieceAt(new Vector2Int(x, y), blackPiecePrefab, -1);

        // Red pieces
        for (int y = 5; y < 8; y++)
            for (int x = 0; x < 8; x++)
                if ((x + y) % 2 == 1)
                    SpawnPieceAt(new Vector2Int(x, y), redPiecePrefab, 1);
    }

    public void SpawnPieceAt(Vector2Int cell, GameObject prefab, int id)
    {
        Vector3Int tilePos = new Vector3Int(cell.x, cell.y, 0);
        Vector3 worldPos = LogicTilemap.GetCellCenterWorld(tilePos);
        GameObject piece = Instantiate(prefab, worldPos, Quaternion.identity, transform);

        // Assign GameManager reference to the piece
        PieceController pc = piece.GetComponent<PieceController>();
        if (pc != null)
            pc.gameManager = this;

        board[cell.x, cell.y] = id;
    }

    // Convert world position to cell coordinates
    public Vector2Int WorldToCell(Vector3 worldPos)
    {
        Vector3Int cellPos = LogicTilemap.WorldToCell(worldPos);
        return new Vector2Int(cellPos.x, cellPos.y);
    }
}
