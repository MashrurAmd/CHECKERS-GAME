using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    [Header("Board")]
    public Tilemap LogicTilemap;
    public int[,] board = new int[8, 8];

    private Vector3Int boardOrigin;

    [Header("Prefabs")]
    public GameObject redPiecePrefab;
    public GameObject blackPiecePrefab;

    void Start()
    {
        // Detect the origin of your 8x8 board in tilemap space
        boardOrigin = LogicTilemap.origin;
        Debug.Log("Board origin: " + boardOrigin);

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
        for (int y = 0; y < 3; y++)
            for (int x = 0; x < 8; x++)
                if ((x + y) % 2 == 1)
                    SpawnPieceAt(new Vector2Int(x, y), blackPiecePrefab, -1);

        for (int y = 5; y < 8; y++)
            for (int x = 0; x < 8; x++)
                if ((x + y) % 2 == 1)
                    SpawnPieceAt(new Vector2Int(x, y), redPiecePrefab, 1);
    }

    public void SpawnPieceAt(Vector2Int cell, GameObject prefab, int id)
    {
        Vector3Int tilePos = new Vector3Int(cell.x + boardOrigin.x, cell.y + boardOrigin.y, 0);
        Vector3 worldPos = LogicTilemap.GetCellCenterWorld(tilePos);
        GameObject piece = Instantiate(prefab, worldPos, Quaternion.identity, transform);

        PieceController pc = piece.GetComponent<PieceController>();
        if (pc != null)
        {
            pc.gameManager = this;
            pc.currentCell = cell;
        }

        board[cell.x, cell.y] = id;
    }

    public Vector2Int WorldToCell(Vector3 worldPos)
    {
        Vector3Int cellPos = LogicTilemap.WorldToCell(worldPos);
        // Adjust with board origin
        return new Vector2Int(cellPos.x - boardOrigin.x, cellPos.y - boardOrigin.y);
    }
}
