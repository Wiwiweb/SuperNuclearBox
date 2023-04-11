using System;
using System.Collections.Generic;
using UnityEngine;
using static LevelManager;
using Random = UnityEngine.Random;

public class LevelRandomGenerator
{
  public class FloorWalker
  {
    public FloorWalker(Vector2Int position, Vector2Int direction)
    {
      this.position = position;
      this.direction = direction;
    }

    public Vector2Int position;
    public Vector2Int direction;
  }

  private class PremadeSquareRoom
  {
    public PremadeSquareRoom(float spawnProbability, int size)
    {
      this.probability = spawnProbability;
      this.size = size;
    }

    public float probability;
    public int size;
  }

  public int FloorsMax = 300;
  public float TurnChanceLeft = 2 / 14f;
  public float TurnChanceRight = 2 / 14f;
  public float TurnChance180 = 1 / 14f;
  public int WalkersMax = 5;
  public float WalkerSpawnChance = 1 / 16f;
  public float WalkerDieChance = 1 / 19f;
  private PremadeSquareRoom[] premadeSquareRooms = new PremadeSquareRoom[] {
    new PremadeSquareRoom(0.3f, 2)
  };

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
    floorWalkers.Add(new FloorWalker(startPoint, RandomDirection()));
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

    void markAsFloor(Vector2Int position)
    {
      if (levelTiles[position.x, position.y] != TileType.Floor)
      {
        levelTiles[position.x, position.y] = TileType.Floor;
        enemySpawnPoints.Add(position);
        nbFloors++;
        minX = Math.Min(minX, position.x);
        maxX = Math.Max(maxX, position.x);
        minY = Math.Min(minY, position.y);
        maxY = Math.Max(maxY, position.y);
      }
    }

    while (nbFloors < FloorsMax && i < 10000)
    {
      foreach (FloorWalker walker in floorWalkers)
      {
        // Mark as floor
        markAsFloor(walker.position);

        // Spawn premade square room
        int roomSize = GetRandomPremadeSquareRoomSize();
        if (roomSize > 0)
        {
          int randomXOffset = Random.Range(0, roomSize);
          int randomYOffset = Random.Range(0, roomSize);
          for (int x = walker.position.x - randomXOffset; x < walker.position.x - randomXOffset + roomSize; x++)
          {
            for (int y = walker.position.y - randomYOffset; y < walker.position.y - randomYOffset + roomSize; y++)
            {
              markAsFloor(new Vector2Int(x, y));
            }
          }
        }

        // Stop level generation if it's big enough
        if (nbFloors >= FloorsMax)
        {
          floorWalkersToDie.Clear();
          floorWalkersToSpawn.Clear();
          break;
        }

        // Die
        float spawnDieChance = Random.Range(0f, 1f);
        if (spawnDieChance < WalkerDieChance)
        {
          if (floorWalkers.Count - floorWalkersToDie.Count + floorWalkersToSpawn.Count > 1)
          {
            floorWalkersToDie.Add(walker);
            boxSpawnPoints.Add(new Vector2Int(walker.position.x, walker.position.y));
          }
        }
        else
        {
          spawnDieChance -= WalkerDieChance;
          // Spawn
          if (floorWalkers.Count - floorWalkersToDie.Count + floorWalkersToSpawn.Count < WalkersMax && spawnDieChance < WalkerSpawnChance)
          {
            floorWalkersToSpawn.Add(new FloorWalker(walker.position, RandomDirection()));
          }

          // Turn
          bool hasTurned;
          (walker.direction, hasTurned) = WeightedRandomRotation(walker.direction);
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
      // DebugLogVisualisation(levelTiles, floorWalkers, minX, maxX, minY, maxY);
    }

    // Find all floor tiles completely surrounded by other floor tiles, for big enemy spawns
    List<Vector2Int> bigEnemySpawnPointsList = new List<Vector2Int>();
    foreach (Vector2Int enemySpawnPoint in enemySpawnPoints)
    {
      if (areAllSurroundingTilesFloor(levelTiles, enemySpawnPoint))
      {
        bigEnemySpawnPointsList.Add(enemySpawnPoint);
      }
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
    bigEnemySpawnPointsList = bigEnemySpawnPointsList.ConvertAll<Vector2Int>(point => point += coordinatesAdjustment);

    return new LevelData(shrunkLevelTiles, spawnCoordinates, boxSpawnPointsList, enemySpawnPointsList, bigEnemySpawnPointsList);
  }

  private Vector2Int RandomDirection()
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

  private (Vector2Int, bool turnedAround) WeightedRandomRotation(Vector2Int direction)
  {
    float rotationChance = Random.Range(0f, 1f);

    if (rotationChance < TurnChanceLeft)
    {
      return (RotateLeft(direction), false);
    }

    rotationChance -= TurnChanceLeft;
    if (rotationChance < TurnChanceRight)
    {
      return (RotateRight(direction), false);
    }

    rotationChance -= TurnChanceRight;
    if (rotationChance < TurnChanceRight)
    {
      return (Rotate180(direction), true);
    }

    return (direction, false);
  }

  private Vector2Int RotateLeft(Vector2Int v)
  {
    return new Vector2Int(v.y * -1, v.x);
  }

  private Vector2Int RotateRight(Vector2Int v)
  {
    return new Vector2Int(v.y, v.x * -1);
  }

  private Vector2Int Rotate180(Vector2Int v)
  {
    return v * -1;
  }

  private int GetRandomPremadeSquareRoomSize()
  {
    float roomChance = Random.Range(0f, 1f);
    foreach (PremadeSquareRoom room in premadeSquareRooms)
    {
      if (roomChance <= room.probability)
      {
        return room.size;
      }
      roomChance -= room.probability;
    }
    return 0;
  }

  private bool areAllSurroundingTilesFloor(TileType[,] levelTiles, Vector2Int tile)
  {
    for (int dx = -1; dx <= 1; dx++)
    {
      for (int dy = -1; dy <= 1; dy++)
      {
        if (levelTiles[tile.x + dx, tile.y + dy] != TileType.Floor)
        {
          return false;
        }
      }
    }
    return true;
  }

  private void DebugLogVisualisation(TileType[,] levelTiles, List<FloorWalker> floorWalkers, int minX, int maxX, int minY, int maxY)
  {
    String s = DebugLogString(levelTiles, floorWalkers, minX, maxX, minY, maxY);
    // Debug.ClearDeveloperConsole();
    // Console.WriteLine(s);
    Debug.Log(s);
    Debug.Break();
  }

  private String DebugLogString(TileType[,] levelTiles, List<FloorWalker> floorWalkers, int minX, int maxX, int minY, int maxY)
  {
    foreach (FloorWalker walker in floorWalkers)
    {
      minX = Math.Min(minX, walker.position.x);
      minY = Math.Min(minY, walker.position.y);
      maxX = Math.Max(maxX, walker.position.x);
      maxY = Math.Max(maxY, walker.position.y);
    }

    Char[,] charArray = new Char[maxX - minX + 1, maxY - minY + 1];
    for (int x = minX; x <= maxX; x++)
    {
      for (int y = minY; y <= maxY; y++)
      {
        if (levelTiles[x, y] == TileType.Floor)
        {
          charArray[x - minX, y - minY] = '#';
        }
        else
        {
          charArray[x - minX, y - minY] = '.';
        }
      }
    }

    foreach (FloorWalker walker in floorWalkers)
    {
      charArray[walker.position.x - minX, walker.position.y - minY] = 'F';
    }

    return Util.CharArrayToString(charArray);
  }
}
