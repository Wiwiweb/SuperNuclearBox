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

  enum TileType
  {
    Ground,
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
  private TileWithProbability[] wallDecorationsUpTiles;
  private TileWithProbability[] wallDecorationsDownTiles;
  private TileWithProbability[] wallDecorationsLeftTiles;
  private TileWithProbability[] wallDecorationsRightTiles;
  private TileWithProbability[] wallDecorationsCornerTopLeftTiles;
  private TileWithProbability[] wallDecorationsCornerTopRightTiles;
  private TileWithProbability[] wallDecorationsCornerBottomLeftTiles;
  private TileWithProbability[] wallDecorationsCornerBottomRightTiles;

  int[,] level_int = { 
    { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, 
    { 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1 }, 
    { 1, 1, 0, 0, 1, 0, 0, 1, 1, 1, 0, 1 }, 
    { 1, 1, 0, 0, 1, 0, 0, 1, 1, 1, 0, 1 }, 
    { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1 }, 
    { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1 }, 
    { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1 }, 
    { 1, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1 }, 
    { 1, 1, 0, 1, 1, 1, 0, 0, 0, 0, 0, 1 }, 
    { 1, 1, 0, 1, 1, 1, 0, 0, 0, 0, 0, 1 }, 
    { 1, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1 }, 
    { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, 
  };

  // Start is called before the first frame update
  void Start()
  {
    floorTilemap = GameObject.Find("Tilemap_Floors").GetComponent<Tilemap>();
    wallTilemap = GameObject.Find("Tilemap_Walls").GetComponent<Tilemap>();
    wallDecorationsUpDownCornersTilemap = GameObject.Find("Tilemap_WallDecorations_UpDownCorners").GetComponent<Tilemap>();
    wallDecorationsSidesTilemap = GameObject.Find("Tilemap_Floors").GetComponent<Tilemap>();

    InitializeTileArrays();

    FillLevelTilemaps();
  }

  // Update is called once per frame
  void Update()
  {
    
  }

  private void InitializeTileArrays()
  {
    floorTiles = InitializeTileArray("Floor tiles/Desert floor", new float[] {0.335f, 0.33f, 0.33f, 0.005f});
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
    Debug.Log(floorTilemap.cellBounds.min);
    Debug.Log(floorTilemap.cellBounds.max);
    for (int x = -floorGridSize; x < floorGridSize; x++)
    {
      for (int y = -floorGridSize; y < floorGridSize; y++)
      {
        floorTilemap.SetTile(new Vector3Int(x, y, 0), getRandomTile(floorTiles));
      }
    }
  }

  private TileBase getRandomTile(TileWithProbability[] tileArray)
  {
    float rand = Random.Range(0f, 1f);
    for (int i = 0; i < tileArray.Length - 1; i++)
    {
      TileWithProbability tileWithProbability = tileArray[i];
      if (rand <= tileWithProbability.probability) {
        return tileWithProbability.tile;
      }
      rand -= tileWithProbability.probability;
    }
    return tileArray[^1].tile;
  }
}
