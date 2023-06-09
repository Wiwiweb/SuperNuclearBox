using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TestScript : MonoBehaviour 
{
    public Tilemap floorTilemap;
    public Tilemap wallTilemap;
    public Tile wallTile;
    public Tile floorTile;

    // Start is called before the first frame update
    void Start() 
	{
        // floorTilemap.SetTile(new Vector3Int(0,0), floorTile);
        wallTilemap.SetTile(new Vector3Int(0,0), wallTile);
    }

    // Update is called once per frame
    void Update() 
	{
        
    }
}
