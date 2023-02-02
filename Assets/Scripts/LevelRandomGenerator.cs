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
  public float TurnChanceLeft = 2/14f;
  public float TurnChanceRight = 2/14f;
  public float TurnChance180 = 1/14f;
  public int WalkersMax = 5;
  public float WalkerSpawnChance = 1/8f;
  public float WalkerDieChance = 1/19f;

  public TileType[,] GenerateRandomLevel()
  {
    TileType[,] level = new TileType[LevelGridSize, LevelGridSize];
    for (int x = 0; x <= level.GetUpperBound(0); x++)
    {
      for (int y = 0; y <= level.GetUpperBound(1); y++)
      {
        level[x, y] = TileType.Wall;
      }
    }

    int center = (LevelGridSize - 1) / 2;
    Vector2Int startPoint = new Vector2Int(center, center);

    List<FloorWalker> floorWalkers = new List<FloorWalker>();
    floorWalkers.Add(new FloorWalker(startPoint, randomDirection()));
    List<FloorWalker> floorWalkersToDie = new List<FloorWalker>();
    List<FloorWalker> floorWalkersToSpawn = new List<FloorWalker>();


    // Main loop
    int i = 0;
    int nbFloors = 0;
    while (nbFloors < FloorsMax && i < 10000)
    {
      foreach (FloorWalker walker in floorWalkers)
      {
        if (level[walker.position.x, walker.position.y] != TileType.Floor)
        {
          level[walker.position.x, walker.position.y] = TileType.Floor;
          nbFloors++;
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
          walker.direction = weightedRandomRotation(walker.direction);

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

    return level;
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

  private Vector2Int weightedRandomRotation(Vector2Int direction)
  {
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
