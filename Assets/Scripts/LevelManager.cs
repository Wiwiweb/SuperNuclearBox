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

  public static int LevelGridSize = 151; // Odd for a center point

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
    GameManager.instance.floorTilemap.ClearAllTiles();
    GameManager.instance.wallTilemap.ClearAllTiles();
    GameManager.instance.wallDecorationTopTilemap.ClearAllTiles();
    GameManager.instance.wallDecorationBottomTilemap.ClearAllTiles();
    GameManager.instance.wallDecorationLeftTilemap.ClearAllTiles();
    GameManager.instance.wallDecorationRightTilemap.ClearAllTiles();
    GameManager.instance.wallDecorationCornerTopLeftTilemap.ClearAllTiles();
    GameManager.instance.wallDecorationCornerTopRightTilemap.ClearAllTiles();
    GameManager.instance.wallDecorationCornerBottomLeftTilemap.ClearAllTiles();
    GameManager.instance.wallDecorationCornerBottomRightTilemap.ClearAllTiles();
  }
}
