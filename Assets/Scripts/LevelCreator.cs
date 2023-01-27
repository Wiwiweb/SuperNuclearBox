using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelCreator : MonoBehaviour
{
  private Tilemap floorTilemap;
  private Tilemap wallTilemap;
  private Tilemap wallDecorationsUpDownCornersTilemap;
  private Tilemap wallDecorationsSidesTilemap;

  private int floorGridSize = 100;

  public enum TileType
  {
    Floor,
    Wall,
  }

  private struct TileWithProbability
  {
    public TileWithProbability(TileBase tile, float probability)
    {
      this.tile = tile;
      this.probability = probability;
    }

    public TileBase tile;
    public float probability;
  }

  private TileWithProbability[] floorTiles;
  private TileWithProbability[] wallTiles;
  private TileWithProbability[] wallDecorationsBottomTiles;
  private TileWithProbability[] wallDecorationsTopTiles;
  private TileWithProbability[] wallDecorationsDownTiles;
  private TileWithProbability[] wallDecorationsLeftTiles;
  private TileWithProbability[] wallDecorationsRightTiles;
  private TileWithProbability[] wallDecorationsCornerTopLeftTiles;
  private TileWithProbability[] wallDecorationsCornerTopRightTiles;
  private TileWithProbability[] wallDecorationsCornerBottomLeftTiles;
  private TileWithProbability[] wallDecorationsCornerBottomRightTiles;

  private int[,] level_int = {
    { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
    { 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1 },
    { 1, 1, 0, 0, 0, 1, 0, 0, 1, 1, 1, 0, 1 },
    { 1, 1, 0, 0, 0, 1, 0, 0, 1, 1, 1, 0, 1 },
    { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1 },
    { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1 },
    { 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1 },
    { 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1 },
    { 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1 },
    { 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1 },
    { 1, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1 },
    { 1, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 1 },
    { 1, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 1 },
    { 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1 },
    { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
  };

  private TileType[,] level;

  // Start is called before the first frame update
  void Start()
  {
    floorTilemap = GameObject.Find("Tilemap_Floors").GetComponent<Tilemap>();
    wallTilemap = GameObject.Find("Tilemap_Walls").GetComponent<Tilemap>();
    wallDecorationsUpDownCornersTilemap = GameObject.Find("Tilemap_WallDecorations_UpDownCorners").GetComponent<Tilemap>();
    wallDecorationsSidesTilemap = GameObject.Find("Tilemap_Floors").GetComponent<Tilemap>();

    level = new TileType[level_int.GetLength(1), level_int.GetLength(0)];
    for (int x = 0; x <= level.GetUpperBound(0); x++)
    {
      for (int y = 0; y <= level.GetUpperBound(1); y++)
      {
        if (level_int[y, x] == 0)
        {
          level[x, level.GetUpperBound(1)-y] = TileType.Floor;
        }
        else
        {
          level[x, level.GetUpperBound(1)-y] = TileType.Wall;
        }
      }
    }

    InitializeTileArrays();

    FillLevelTilemaps();
  }

  // Update is called once per frame
  void Update()
  {

  }

  private void InitializeTileArrays()
  {
    floorTiles = InitializeTileArray("Floor tiles/Desert floor", new float[] { 0.335f, 0.33f, 0.33f, 0.005f });
    wallTiles = InitializeTileArray("Wall tiles/Desert wall", new float[] { 0.25f, 0.25f, 0.25f, 0.25f });
    wallDecorationsBottomTiles = InitializeTileArray("Wall decorations tiles/Desert wall decoration bottom", new float[] { 0.25f, 0.25f, 0.25f, 0.25f });
    wallDecorationsTopTiles = InitializeTileArray("Wall decorations tiles/Desert wall decoration top", new float[] { 1f });
    wallDecorationsLeftTiles = InitializeTileArray("Wall decorations tiles/Desert wall decoration left", new float[] { 1f });
    wallDecorationsRightTiles = InitializeTileArray("Wall decorations tiles/Desert wall decoration right", new float[] { 1f });
    wallDecorationsCornerTopLeftTiles = InitializeTileArray("Wall decorations tiles/Desert wall decoration corner top left", new float[] { 1f });
    wallDecorationsCornerTopRightTiles = InitializeTileArray("Wall decorations tiles/Desert wall decoration corner top right", new float[] { 1f });
    // wallDecorationsCornerBottomLeftTiles = InitializeTileArray("Wall decorations tiles/Desert wall decoration corner bottom left", new float[] {1f});
    wallDecorationsCornerBottomRightTiles = InitializeTileArray("Wall decorations tiles/Desert wall decoration corner bottom right", new float[] { 1f });
  }

  private TileWithProbability[] InitializeTileArray(string tilePrefix, float[] probabilities)
  {
    TileWithProbability[] tileArray = new TileWithProbability[probabilities.Length];
    for (int i = 0; i < probabilities.Length; i++)
    {
      string tilePath = "Tiles/" + tilePrefix + "_" + i;
      TileBase tile = Resources.Load<TileBase>(tilePath);
      tileArray[i] = new TileWithProbability(tile, probabilities[i]);
    }
    return tileArray;
  }


  private void FillLevelTilemaps()
  {
    // Floors
    for (int x = 0; x < floorGridSize; x++)
    {
      for (int y = 0; y < floorGridSize; y++)
      {
        floorTilemap.SetTile(new Vector3Int(x, y, 0), getRandomTile(floorTiles));
      }
    }

    // Walls
    int levelOffsetX = floorGridSize - (level.GetLength(0) / 2);
    int levelOffsetY = floorGridSize - (level.GetLength(1) / 2);
    for (int x = 0; x <= level.GetUpperBound(0); x++)
    {
      for (int y = 0; y <= level.GetUpperBound(1); y++)
      {
        if (level[x,y] == TileType.Wall)
        {
          wallTilemap.SetTile(new Vector3Int(x + levelOffsetX, y + levelOffsetY, 0), getRandomTile(wallTiles));
        }     
      }
    }
  }

  private TileBase getRandomTile(TileWithProbability[] tileArray)
  {
    float rand = Random.Range(0f, 1f);
    for (int i = 0; i < tileArray.Length - 1; i++)
    {
      TileWithProbability tileWithProbability = tileArray[i];
      if (rand <= tileWithProbability.probability)
      {
        return tileWithProbability.tile;
      }
      rand -= tileWithProbability.probability;
    }
    return tileArray[^1].tile;
  }
}
