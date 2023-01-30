using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour
{
  public enum TileType
  {
    Floor,
    Wall,
  }

  public static int LevelGridSize = 201; // Odd for a center point

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
    GameObject player = Instantiate(playerPrefabTemp, playerSpawnPosition, Quaternion.identity);
    FollowPlayer cameraScript = cameraTemp.GetComponent<FollowPlayer>();
    cameraScript.player = player;
  }

  // void Update()
  // {

  // }
}
