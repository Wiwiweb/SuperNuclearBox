using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Tilemaps;
using Debug = UnityEngine.Debug;

public static class LevelManager
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
    public List<Vector2Int> boxSpawnPoints;
    public List<Vector2Int> enemySpawnPoints;

    public LevelData(TileType[,] tiles, Vector2Int spawnPointTile, List<Vector2Int> boxSpawnPoints, List<Vector2Int> enemySpawnPoints)
    {
      this.tiles = tiles;
      this.spawnPointTile = spawnPointTile;
      this.boxSpawnPoints = boxSpawnPoints;
      this.enemySpawnPoints = enemySpawnPoints;
    }
  }

  public static int LevelGridSize = 301; // Odd for a center point

  public static LevelData level;

  public static void CreateLevel()
  {
    LevelRandomGenerator levelRandomGenerator = new LevelRandomGenerator();
    level = levelRandomGenerator.GenerateRandomLevel();

    LevelTileLayer levelTileLayer = new LevelTileLayer(level.tiles);
    levelTileLayer.FillLevelTilemaps();
  }

  public static void DestroyLevel()
  {
    level = null;
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
  }
}
