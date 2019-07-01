using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShrinelandsTactics;
using UnityEngine.Tilemaps;

public class CombatManager : MonoBehaviour
{
    public DungeonMaster DM;
    public Tilemap tileMap;
    public Tile basicTile;
    // Start is called before the first frame update
    void Start()
    {
        DM = new DungeonMaster();

        tileMap.ClearAllTiles();
        tileMap.SetTile(Vector3Int.zero, basicTile);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
