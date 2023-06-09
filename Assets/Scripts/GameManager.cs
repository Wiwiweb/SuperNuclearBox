using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class GameManager : MonoBehaviour
{
  public const int BoxesBeforeLevelSwitch = 10;

  public static GameManager instance;

  public Tilemap floorTilemap;
  public Tilemap wallTilemap;
  public Tilemap wallDecorationTopTilemap;
  public Tilemap wallDecorationBottomTilemap;
  public Tilemap wallDecorationLeftTilemap;
  public Tilemap wallDecorationRightTilemap;
  public Tilemap wallDecorationCornerTopLeftTilemap;
  public Tilemap wallDecorationCornerTopRightTilemap;
  public Tilemap wallDecorationCornerBottomLeftTilemap;
  public Tilemap wallDecorationCornerBottomRightTilemap;

  public GameObject box;
  private GameObject player;
  private PlayerController playerController;
  private PlayerInputComponent playerInputComponent;

  [SerializeField]
  private GameObject playerPrefab;
  [SerializeField]
  private GameObject boxPrefab;
  [SerializeField]
  private GameObject floatingTextPrefab;
  [SerializeField]
  private float minBoxSpawnDistanceToPlayer = 2;


  private CameraController cameraController;
  private Coroutine hitStopRoutine;

  public GameObject Player
  {
    get => player;
    set { player = value; playerController = player.GetComponent<PlayerController>(); }
  }


  void Awake()
  {
    instance = this;
  }

  void Start()
  {
    cameraController = Camera.main.GetComponent<CameraController>();

    MusicController.instance.UnPause(); // In case we restarted while the music was paused
    Hitstop.ClearCallbacks(); // In case we restarted while hitstop was happening
    LevelManager.CreateLevel();
    SpawnPlayer();
    SpawnBox();
    EnemySpawnManager.Init();
    EnemySpawnManager.UpdateEnemySpawns();
    GunManager.Init();
    UIController.instance.UpdateScoreLabels(PersistentData.BoxScore, PersistentData.BestBoxScore);
    cameraController.SetPosition(Player.transform.position); // Ease the first frame camera movement
  }

  void Update()
  {
    Hitstop.Update();
    if (!playerController.Dead)
    {
      EnemySpawnManager.UpdateEnemySpawns();
    }
  }

  private void NewLevel()
  {
    LevelManager.CreateLevel();
    SpawnPlayer();
    SpawnBox();
    EnemySpawnManager.ResetEnemyPoints();
    EnemySpawnManager.UpdateEnemySpawns();
  }

  private void SpawnPlayer()
  {
    Vector2Int spawnPointTile = LevelManager.level.spawnPointTile;
    Vector3 playerSpawnPosition = Util.WorldPositionFromTile(spawnPointTile);
    Player = Instantiate(playerPrefab, playerSpawnPosition, Quaternion.identity);
    cameraController.Player = Player;
    OnScreenSticksManager.instance.Player = Player;
  }

  public void SpawnBox()
  {
    List<Vector2Int> possibleSpawnTiles = LevelManager.level.boxSpawnPoints;
    Vector3 boxSpawnPosition;
    do
    {
      Vector2Int chosenSpawnTile = Util.RandomFromList(possibleSpawnTiles);
      boxSpawnPosition = Util.WorldPositionFromTile(chosenSpawnTile);
    } while (Vector3.Distance(boxSpawnPosition, Player.transform.position) < minBoxSpawnDistanceToPlayer);

    box = Instantiate(boxPrefab, boxSpawnPosition, Quaternion.identity);
  }

  public void IncrementBoxScore()
  {
    PersistentData.BoxScore++;
    if (PersistentData.BoxScore > PersistentData.BestBoxScore)
    {
      PersistentData.BestBoxScore = PersistentData.BoxScore;
    }
    UIController.instance.UpdateScoreLabels(PersistentData.BoxScore, PersistentData.BestBoxScore);
  }

  public void CreateFloatingText(Vector3 floatingTextPosition, string text)
  {
    GameObject floatingText = GameObject.Instantiate(floatingTextPrefab, floatingTextPosition, Quaternion.identity);
    floatingText.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(text);
  }
}
