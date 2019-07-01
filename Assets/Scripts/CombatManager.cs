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

    public TextAsset text;
    // Start is called before the first frame update
    void Start()
    {
        var data = GameData.CreateFromJson(text.text);
        DM = new DungeonMaster(data);

        ShrinelandsTactics.World.Tile t = new ShrinelandsTactics.World.Tile("test");
        Debug.Log(t.json);

        tileMap.ClearAllTiles();
        tileMap.SetTile(Vector3Int.zero, basicTile);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
