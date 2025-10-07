using UnityEngine;
using UnityEngine.Tilemaps;

public class DiceSpawner : MonoBehaviour
{
    public GameObject dicePrefab; // Assign your dice prefab here
    public Tilemap tilemap;       // Assign your tilemap here
    public Vector3Int spawnCell = new Vector3Int(0, 0, 0); // Where to spawn

    void Start()
    {
        SpawnDice(spawnCell);
    }

    public void SpawnDice(Vector3Int cellPosition)
    {
        Vector3 spawnPos = tilemap.GetCellCenterWorld(cellPosition);
        GameObject dice = Instantiate(dicePrefab, spawnPos, Quaternion.identity);

        // Assign Tilemap reference to dice script
        DiceController controller = dice.GetComponent<DiceController>();
        if (controller != null)
        {
            controller.tilemap = tilemap;
        }
    }
}
