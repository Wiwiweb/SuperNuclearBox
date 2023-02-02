using System;
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

  public static int LevelGridSize = 301; // Odd for a center point

  public GameObject playerPrefabTemp;
  public GameObject cameraTemp;

  // Start is called before the first frame update
  void Start()
  {
    LevelRandomGenerator levelRandomGenerator = new LevelRandomGenerator();
    TileType[,] level = levelRandomGenerator.GenerateRandomLevel();

    LevelTileLayer levelTileLayer = new LevelTileLayer(level);
    levelTileLayer.FillLevelTilemaps();

    Tilemap wallTilemap = GameObject.Find("Tilemap_Walls").GetComponent<Tilemap>();
    int centerTile = (LevelGridSize - 1) / 2;
    Vector3 playerSpawnPosition = wallTilemap.GetCellCenterWorld(new Vector3Int(centerTile, centerTile, 1));
    playerSpawnPosition.z = 1;
    GameObject player = Instantiate(playerPrefabTemp, playerSpawnPosition, Quaternion.identity);
    FollowPlayer cameraScript = cameraTemp.GetComponent<FollowPlayer>();
    cameraScript.player = player;
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
      TileType[,] level = levelRandomGenerator.GenerateRandomLevel();
      LevelTileLayer levelTileLayer = new LevelTileLayer(level);
      levelTileLayer.FillLevelTilemaps();
    }

  }

  public static void printStopwatch(Stopwatch stopwatch)
  {
    stopwatch.Stop();
    Debug.Log($"Stopwatch: {stopwatch.ElapsedMilliseconds}");
    stopwatch.Reset();
    stopwatch.Start();
  }
}
