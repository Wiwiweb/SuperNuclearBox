using System;
using System.Collections.Generic;
using UnityEngine;
using static LevelManager;
using Random = UnityEngine.Random;

public class LevelRandomGenerator
{
  private class FloorWalker
  {
    public FloorWalker(Vector2Int position, Vector2Int direction)
    {
      this.position = position;
      this.direction = direction;
    }

    public Vector2Int position;
    public Vector2Int direction;
  }

  public int FloorsMax = 300;
  public float TurnChanceLeft = 2 / 14f;
  public float TurnChanceRight = 2 / 14f;
  public float TurnChance180 = 1 / 14f;
  public int WalkersMax = 5;
  public float WalkerSpawnChance = 1 / 8f;
  public float WalkerDieChance = 1 / 19f;

  public LevelData GenerateRandomLevel()
  {
    TileType[,] levelTiles = new TileType[LevelGridSize, LevelGridSize];
    for (int x = 0; x <= levelTiles.GetUpperBound(0); x++)
    {
      for (int y = 0; y <= levelTiles.GetUpperBound(1); y++)
      {
        levelTiles[x, y] = TileType.Wall;
      }
    }

    int center = (LevelGridSize - 1) / 2;
    Vector2Int startPoint = new Vector2Int(center, center);

    List<FloorWalker> floorWalkers = new List<FloorWalker>();
    floorWalkers.Add(new FloorWalker(startPoint, randomDirection()));
    List<FloorWalker> floorWalkersToDie = new List<FloorWalker>();
    List<FloorWalker> floorWalkersToSpawn = new List<FloorWalker>();

    ISet<Vector2Int> enemySpawnPoints = new HashSet<Vector2Int>();
    ISet<Vector2Int> boxSpawnPoints = new HashSet<Vector2Int>();


    // Main loop
    int i = 0;
    int nbFloors = 0;
    int minX = center;
    int maxX = center;
    int minY = center;
    int maxY = center;
    while (nbFloors < FloorsMax && i < 10000)
    {
      foreach (FloorWalker walker in floorWalkers)
      {
        if (levelTiles[walker.position.x, walker.position.y] != TileType.Floor)
        {
          levelTiles[walker.position.x, walker.position.y] = TileType.Floor;
          enemySpawnPoints.Add(new Vector2Int(walker.position.x, walker.position.y));
          nbFloors++;
          minX = Math.Min(minX, walker.position.x);
          maxX = Math.Max(maxX, walker.position.x);
          minY = Math.Min(minY, walker.position.y);
          maxY = Math.Max(maxY, walker.position.y);
          if (nbFloors >= FloorsMax)
          {
            floorWalkersToDie.Clear();
            floorWalkersToSpawn.Clear();
            break;
          }
        }
        // Die
        float spawnDieChance = Random.Range(0f, 1f);
        if (floorWalkers.Count - floorWalkersToDie.Count + floorWalkersToSpawn.Count > 1 && spawnDieChance < WalkerDieChance)
        {
          floorWalkersToDie.Add(walker);
          boxSpawnPoints.Add(new Vector2Int(walker.position.x, walker.position.y));
        }
        else
        {
          spawnDieChance -= WalkerDieChance;
          // Spawn
          if (floorWalkers.Count - floorWalkersToDie.Count + floorWalkersToSpawn.Count < WalkersMax && spawnDieChance < WalkerSpawnChance)
          {
            floorWalkersToSpawn.Add(new FloorWalker(walker.position, randomDirection()));
          }

          // Turn
          bool hasTurned;
          walker.direction = weightedRandomRotation(walker.direction, out hasTurned);
          if (hasTurned)
          {
            boxSpawnPoints.Add(new Vector2Int(walker.position.x, walker.position.y));
          }

          // Move
          walker.position += walker.direction;
        }
      }

      foreach (FloorWalker walker in floorWalkersToDie)
      {
        floorWalkers.Remove(walker);
      }
      foreach (FloorWalker walker in floorWalkersToSpawn)
      {
        floorWalkers.Add(walker);
      }
      floorWalkersToDie.Clear();
      floorWalkersToSpawn.Clear();

      i++;
    }

    // Include boundary walls
    minX -= 1;
    minY -= 1;
    maxX += 1;
    maxY += 1;

    // Shrink level array for faster tile laying
    TileType[,] shrunkLevelTiles = new TileType[maxX - minX + 1, maxY - minY + 1];
    for (int x = 0; x <= shrunkLevelTiles.GetUpperBound(0); x++)
    {
      for (int y = 0; y <= shrunkLevelTiles.GetUpperBound(1); y++)
      {
        shrunkLevelTiles[x, y] = levelTiles[x + minX, y + minY];
      }
    }

    // Now that we have shrunk the level, it will be recentered.
    // Adjust the coordinates of spawn points.
    Vector2Int originaLevelOffset = new Vector2Int(minX, minY); // Offset of level during generation
    Vector2Int newLevelOffset = new Vector2Int(
      LevelGridSize / 2 - (shrunkLevelTiles.GetLength(0) / 2),
      LevelGridSize / 2 - (shrunkLevelTiles.GetLength(1) / 2)
    ); // Offset of level after re-centering
    Vector2Int coordinatesAdjustment = newLevelOffset - originaLevelOffset;
    Vector2Int spawnCoordinates = startPoint + coordinatesAdjustment;

    List<Vector2Int> boxSpawnPointsList = new List<Vector2Int>(boxSpawnPoints);
    List<Vector2Int> enemySpawnPointsList = new List<Vector2Int>(enemySpawnPoints);
    boxSpawnPointsList = boxSpawnPointsList.ConvertAll<Vector2Int>(point => point += coordinatesAdjustment);
    enemySpawnPointsList = enemySpawnPointsList.ConvertAll<Vector2Int>(point => point += coordinatesAdjustment);

    return new LevelData(shrunkLevelTiles, spawnCoordinates, boxSpawnPointsList, enemySpawnPointsList);
  }

  private Vector2Int randomDirection()
  {
    switch (Random.Range(0, 4))
    {
      case 0:
        return Vector2Int.up;
      case 1:
        return Vector2Int.down;
      case 2:
        return Vector2Int.left;
      case 3:
        return Vector2Int.right;
      default:
        throw new ArgumentOutOfRangeException();
    }
  }

  private Vector2Int weightedRandomRotation(Vector2Int direction, out bool spawnBox)
  {
    spawnBox = false;
    float rotationChance = Random.Range(0f, 1f);

    if (rotationChance < TurnChanceLeft)
    {
      return rotateLeft(direction);
    }

    rotationChance -= TurnChanceLeft;
    if (rotationChance < TurnChanceRight)
    {
      return rotateRight(direction);
    }

    rotationChance -= TurnChanceRight;
    if (rotationChance < TurnChanceRight)
    {
      spawnBox = false;
      return rotate180(direction);
    }

    return direction;
  }

  private Vector2Int rotateLeft(Vector2Int v)
  {
    return new Vector2Int(v.y * -1, v.x);
  }

  private Vector2Int rotateRight(Vector2Int v)
  {
    return new Vector2Int(v.y, v.x * -1);
  }

  private Vector2Int rotate180(Vector2Int v)
  {
    return v * -1;
  }
}
