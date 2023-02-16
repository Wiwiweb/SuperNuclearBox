using UnityEngine;
using UnityEngine.Tilemaps;
using static LevelManager;

public class LevelTileLayer
{
  private Tilemap floorTilemap;
  private Tilemap wallTilemap;
  private Tilemap wallDecorationTopTilemap;
  private Tilemap wallDecorationBottomTilemap;
  private Tilemap wallDecorationLeftTilemap;
  private Tilemap wallDecorationRightTilemap;
  private Tilemap wallDecorationCornerTopLeftTilemap;
  private Tilemap wallDecorationCornerTopRightTilemap;
  private Tilemap wallDecorationCornerBottomLeftTilemap;
  private Tilemap wallDecorationCornerBottomRightTilemap;

  private TileBase floorTile;
  private TileBase wallTile;
  private TileBase wallDecorationsBottomTile;
  private TileBase wallDecorationsTopTile;
  private TileBase wallDecorationsLeftTile;
  private TileBase wallDecorationsRightTile;
  private TileBase wallDecorationsCornerTopLeftTile;
  private TileBase wallDecorationsCornerTopRightTile;
  private TileBase wallDecorationsCornerBottomLeftTile;
  private TileBase wallDecorationsCornerBottomRightTile;

  private TileType[,] level;

  public LevelTileLayer(TileType[,] level)
  {
    this.level = level;
    InitializeResources();
  }

  void InitializeResources()
  {
    floorTilemap = GameManager.instance.floorTilemap;
    wallTilemap = GameManager.instance.wallTilemap;
    wallDecorationTopTilemap = GameManager.instance.wallDecorationTopTilemap;
    wallDecorationBottomTilemap = GameManager.instance.wallDecorationBottomTilemap;
    wallDecorationLeftTilemap = GameManager.instance.wallDecorationLeftTilemap;
    wallDecorationRightTilemap = GameManager.instance.wallDecorationRightTilemap;
    wallDecorationCornerTopLeftTilemap = GameManager.instance.wallDecorationCornerTopLeftTilemap;
    wallDecorationCornerTopRightTilemap = GameManager.instance.wallDecorationCornerTopRightTilemap;
    wallDecorationCornerBottomLeftTilemap = GameManager.instance.wallDecorationCornerBottomLeftTilemap;
    wallDecorationCornerBottomRightTilemap = GameManager.instance.wallDecorationCornerBottomRightTilemap;

    floorTile = Resources.Load<TileBase>("Tiles/Floor tiles/Desert floor");
    wallTile = Resources.Load<TileBase>("Tiles/Wall tiles/Desert wall");
    wallDecorationsBottomTile = Resources.Load<TileBase>("Tiles/Wall decorations tiles/Desert wall decoration bottom");
    wallDecorationsTopTile = Resources.Load<TileBase>("Tiles/Wall decorations tiles/Desert wall decoration top");
    wallDecorationsLeftTile = Resources.Load<TileBase>("Tiles/Wall decorations tiles/Desert wall decoration left");
    wallDecorationsRightTile = Resources.Load<TileBase>("Tiles/Wall decorations tiles/Desert wall decoration right");
    wallDecorationsCornerTopLeftTile = Resources.Load<TileBase>("Tiles/Wall decorations tiles/Desert wall decoration corner top left");
    // wallDecorationsCornerTopRightTile = Resources.Load<TileBase>("Tiles/Wall decorations tiles/Desert wall decoration corner top right");
    wallDecorationsCornerTopRightTile = null;
    wallDecorationsCornerBottomLeftTile = Resources.Load<TileBase>("Wall decorations tiles/Desert wall decoration corner bottom left");
    wallDecorationsCornerBottomRightTile = Resources.Load<TileBase>("Tiles/Wall decorations tiles/Desert wall decoration corner bottom right");
  }

  public void FillLevelTilemaps()
  {
    int floorGridSize = LevelGridSize / 2;
    floorTilemap.size = new Vector3Int(floorGridSize, floorGridSize, 1);
    wallTilemap.size = new Vector3Int(LevelGridSize, LevelGridSize, 1);

    // Floors 
    floorTilemap.FloodFill(new Vector3Int(0, 0, 0), floorTile);

    // Walls
    int levelOffsetX = LevelGridSize / 2 - (level.GetLength(0) / 2);
    int levelOffsetY = LevelGridSize / 2 - (level.GetLength(1) / 2);
    for (int x = 0; x <= level.GetUpperBound(0); x++)
    {
      for (int y = 0; y <= level.GetUpperBound(1); y++)
      {
        int tilemapX = x + levelOffsetX;
        int tilemapY = y + levelOffsetY;
        if (level[x, y] == TileType.Wall)
        {
          wallTilemap.SetTile(new Vector3Int(tilemapX, tilemapY, 0), wallTile);

          // Wall decorations
          checkAndPlaceWallDecorationWithCorners(x, y + 1, tilemapX, tilemapY + 1, wallDecorationTopTilemap, wallDecorationsTopTile, wallDecorationCornerTopLeftTilemap, wallDecorationCornerTopRightTilemap, wallDecorationsCornerTopLeftTile, wallDecorationsCornerTopRightTile);
          checkAndPlaceWallDecorationWithCorners(x, y - 1, tilemapX, tilemapY - 1, wallDecorationBottomTilemap, wallDecorationsBottomTile, wallDecorationCornerBottomLeftTilemap, wallDecorationCornerBottomRightTilemap, wallDecorationsCornerBottomLeftTile, wallDecorationsCornerBottomRightTile);
          checkAndPlaceWallDecoration(x - 1, y, tilemapX - 1, tilemapY, wallDecorationLeftTilemap, wallDecorationsLeftTile);
          checkAndPlaceWallDecoration(x + 1, y, tilemapX + 1, tilemapY, wallDecorationRightTilemap, wallDecorationsRightTile);
        }
      }
    }
    wallTilemap.FloodFill(new Vector3Int(0, 0, 0), wallTile);
  }

  private void checkAndPlaceWallDecoration(int x, int y, int tilemapX, int tilemapY, Tilemap tilemap, TileBase tile)
  {
    if (0 <= x && x <= level.GetUpperBound(0) && 0 <= y && y <= level.GetUpperBound(1) && level[x, y] == TileType.Floor)
    {
      tilemap.SetTile(new Vector3Int(tilemapX, tilemapY, 0), tile);
    }
  }

  private void checkAndPlaceWallDecorationWithCorners(int x, int y, int tilemapX, int tilemapY, Tilemap tilemap, TileBase tile, Tilemap tilemapCornerLeft, Tilemap tilemapCornerRight, TileBase tileCornerLeft, TileBase tileCornerRight)
  {
    if (0 <= x && x <= level.GetUpperBound(0) && 0 <= y && y <= level.GetUpperBound(1) && level[x, y] == TileType.Floor)
    {
      tilemap.SetTile(new Vector3Int(tilemapX, tilemapY, 0), tile);
      if (0 <= x - 1 && level[x - 1, y] == TileType.Floor && level[x - 1, y - 1] == TileType.Floor)
      {
        tilemapCornerLeft.SetTile(new Vector3Int(tilemapX - 1, tilemapY, 0), tileCornerLeft);
      }
      if (x + 1 <= level.GetUpperBound(0) && level[x + 1, y] == TileType.Floor && level[x + 1, y - 1] == TileType.Floor)
      {
        tilemapCornerRight.SetTile(new Vector3Int(tilemapX + 1, tilemapY, 0), tileCornerRight);
      }
    }
  }
}
