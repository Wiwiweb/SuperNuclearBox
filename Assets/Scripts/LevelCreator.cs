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

  private TileBase floorTile;
  private TileBase wallTile;
  private TileBase wallDecorationsBottomTiles;
  private TileBase wallDecorationsTopTiles;
  private TileBase wallDecorationsDownTiles;
  private TileBase wallDecorationsLeftTiles;
  private TileBase wallDecorationsRightTiles;
  private TileBase wallDecorationsCornerTopLeftTiles;
  private TileBase wallDecorationsCornerTopRightTiles;
  private TileBase wallDecorationsCornerBottomLeftTiles;
  private TileBase wallDecorationsCornerBottomRightTiles;

  public GameObject playerPrefabTemp;
  public GameObject cameraTemp;

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

    GameObject player = Instantiate(playerPrefabTemp, new Vector3(17,16,1), Quaternion.identity);
    FollowPlayer cameraScript = cameraTemp.GetComponent<FollowPlayer>();
    cameraScript.player = player;
  }

  // Update is called once per frame
  void Update()
  {

  }

  private void InitializeTileArrays()
  {
    floorTile = Resources.Load<TileBase>("Tiles/Floor tiles/Desert floor");
    wallTile = Resources.Load<TileBase>("Tiles/Wall tiles/Desert wall");
    wallDecorationsBottomTiles = Resources.Load<TileBase>("Tiles/Wall decorations tiles/Desert wall decoration bottom");
    wallDecorationsTopTiles = Resources.Load<TileBase>("Tiles/Wall decorations tiles/Desert wall decoration top");
    wallDecorationsLeftTiles = Resources.Load<TileBase>("Tiles/Wall decorations tiles/Desert wall decoration left");
    wallDecorationsRightTiles = Resources.Load<TileBase>("Tiles/Wall decorations tiles/Desert wall decoration right");
    wallDecorationsCornerTopLeftTiles = Resources.Load<TileBase>("Tiles/Wall decorations tiles/Desert wall decoration corner top left");
    wallDecorationsCornerTopRightTiles = Resources.Load<TileBase>("Tiles/Wall decorations tiles/Desert wall decoration corner top right");
    // wallDecorationsCornerBottomLeftTiles = InitializeTileArray("Wall decorations tiles/Desert wall decoration corner bottom left", new float[] {1f});
    wallDecorationsCornerBottomRightTiles = Resources.Load<TileBase>("Tiles/Wall decorations tiles/Desert wall decoration corner bottom right");
  }

  private void FillLevelTilemaps()
  {
    floorTilemap.size = new Vector3Int(floorGridSize, floorGridSize, 1);
    wallTilemap.size = new Vector3Int(floorGridSize*2, floorGridSize*2, 1);

    // Floors
    floorTilemap.FloodFill(new Vector3Int(0, 0, 0), floorTile);

    // Walls
    int levelOffsetX = floorGridSize - (level.GetLength(0) / 2);
    int levelOffsetY = floorGridSize - (level.GetLength(1) / 2);
    for (int x = 0; x <= level.GetUpperBound(0); x++)
    {
      for (int y = 0; y <= level.GetUpperBound(1); y++)
      {
        if (level[x,y] == TileType.Wall)
        {
          wallTilemap.SetTile(new Vector3Int(x + levelOffsetX, y + levelOffsetY, 0), wallTile);
        }     
      }
    }
    wallTilemap.FloodFill(new Vector3Int(0, 0, 0), wallTile);
  }
}
