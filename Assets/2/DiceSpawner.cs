using UnityEngine;
using UnityEngine.Tilemaps;

public class DiceSpawner : MonoBehaviour
{
    public GameObject dicePrefab; // Assign your dice prefab here
    public Tilemap tilemap;       // Assign your tilemap here

    // List of starting spawn positions for multiple dice
    private Vector3Int[] spawnPositions = new Vector3Int[]
    {
        new Vector3Int(-4, -4, 0),
        new Vector3Int(-2, -4, 0),
        new Vector3Int(0, -4, 0),
        new Vector3Int(2, -4, 0)
    };

    void Start()
    {
        SpawnAllDice();
    }

    void SpawnAllDice()
    {
        foreach (var cell in spawnPositions)
        {
            SpawnDice(cell);
        }
    }

    public void SpawnDice(Vector3Int cellPosition)
    {
        Vector3 spawnPos = tilemap.GetCellCenterWorld(cellPosition);
        GameObject dice = Instantiate(dicePrefab, spawnPos, Quaternion.identity);

        // Assign Tilemap reference to each dice
        DiceController controller = dice.GetComponent<DiceController>();
        if (controller != null)
        {
            controller.tilemap = tilemap;
        }
    }
}
