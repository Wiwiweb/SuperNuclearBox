using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using Debug = UnityEngine.Debug;

public class LevelManager : MonoBehaviour
{
  public enum TileType
  {
    Floor,
    Wall,
  }

  public class LevelData
  {
    public TileType[,] tiles;
    public Vector2Int spawnPointTile;
    // public List<Vector2> boxSpawnPoints;

    public LevelData(TileType[,] tiles, Vector2Int spawnPointTile)
    {
      this.tiles = tiles;
      this.spawnPointTile = spawnPointTile;
    }
  }

  public static int LevelGridSize = 301; // Odd for a center point

  public GameObject playerPrefabTemp;
  public GameObject cameraTemp;

  [SerializeField]
  private GameObject player;

  // Start is called before the first frame update
  void Start()
  {
    LevelRandomGenerator levelRandomGenerator = new LevelRandomGenerator();
    LevelData level = levelRandomGenerator.GenerateRandomLevel();

    LevelTileLayer levelTileLayer = new LevelTileLayer(level.tiles);
    levelTileLayer.FillLevelTilemaps();

    spawnPlayer(level.spawnPointTile);
  }

  void Update()
  {
    if (Keyboard.current.rKey.wasPressedThisFrame)
    {
      GameObject.Find("Tilemap_Floors").GetComponent<Tilemap>().ClearAllTiles();
      GameObject.Find("Tilemap_Walls").GetComponent<Tilemap>().ClearAllTiles();
      GameObject.Find("Tilemap_WallDecorations_Top").GetComponent<Tilemap>().ClearAllTiles();
      GameObject.Find("Tilemap_WallDecorations_Bottom").GetComponent<Tilemap>().ClearAllTiles();
      GameObject.Find("Tilemap_WallDecorations_Left").GetComponent<Tilemap>().ClearAllTiles();
      GameObject.Find("Tilemap_WallDecorations_Right").GetComponent<Tilemap>().ClearAllTiles();
      GameObject.Find("Tilemap_WallDecorations_Corner_Top_Left").GetComponent<Tilemap>().ClearAllTiles();
      GameObject.Find("Tilemap_WallDecorations_Corner_Top_Right").GetComponent<Tilemap>().ClearAllTiles();
      GameObject.Find("Tilemap_WallDecorations_Corner_Bottom_Left").GetComponent<Tilemap>().ClearAllTiles();
      GameObject.Find("Tilemap_WallDecorations_Corner_Bottom_Right").GetComponent<Tilemap>().ClearAllTiles();

      LevelRandomGenerator levelRandomGenerator = new LevelRandomGenerator();
      LevelData level = levelRandomGenerator.GenerateRandomLevel();
      LevelTileLayer levelTileLayer = new LevelTileLayer(level.tiles);
      levelTileLayer.FillLevelTilemaps();

      Destroy(player);
      spawnPlayer(level.spawnPointTile);
    }

  }

  public static void printStopwatch(Stopwatch stopwatch)
  {
    stopwatch.Stop();
    Debug.Log($"Stopwatch: {stopwatch.ElapsedMilliseconds}");
    stopwatch.Reset();
    stopwatch.Start();
  }

  private void spawnPlayer(Vector2Int spawnPointTile)
  {
    Tilemap wallTilemap = GameObject.Find("Tilemap_Walls").GetComponent<Tilemap>();
    Vector3 playerSpawnPosition = wallTilemap.GetCellCenterWorld(new Vector3Int(spawnPointTile.x, spawnPointTile.y, 1));
    playerSpawnPosition.z = 1;
    player = Instantiate(playerPrefabTemp, playerSpawnPosition, Quaternion.identity);
    FollowPlayer cameraScript = cameraTemp.GetComponent<FollowPlayer>();
    cameraScript.player = player;
  }
}
