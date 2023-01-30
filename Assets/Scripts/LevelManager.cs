using UnityEngine;

public class LevelManager : MonoBehaviour
{
  public enum TileType
  {
    Floor,
    Wall,
  }

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
    { 1, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 1 },
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
    level = new TileType[level_int.GetLength(1), level_int.GetLength(0)];
    for (int x = 0; x <= level.GetUpperBound(0); x++)
    {
      for (int y = 0; y <= level.GetUpperBound(1); y++)
      {
        if (level_int[y, x] == 0)
        {
          level[x, level.GetUpperBound(1) - y] = TileType.Floor;
        }
        else
        {
          level[x, level.GetUpperBound(1) - y] = TileType.Wall;
        }
      }
    }

    LevelTileLayer levelTileLayer = new LevelTileLayer(level);
    levelTileLayer.FillLevelTilemaps();

    GameObject player = Instantiate(playerPrefabTemp, new Vector3(15.5f, 16, 1), Quaternion.identity);
    FollowPlayer cameraScript = cameraTemp.GetComponent<FollowPlayer>();
    cameraScript.player = player;
  }
  
  // void Update()
  // {

  // }
}
