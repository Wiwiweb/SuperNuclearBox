using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
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
  public GameObject player;
  public bool dead = false;

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
  private int boxScore = 0;


  void Awake() {
      instance = this;
  }

  void Start()
  {
    cameraController = Camera.main.GetComponent<CameraController>();

    LevelManager.CreateLevel();
    SpawnPlayer();
    SpawnBox();
    EnemySpawnManager.Init();
    EnemySpawnManager.UpdateEnemySpawns();
    GunManager.Init();
    UIController.instance.UpdateScoreLabels(boxScore, PersistentData.BestBoxScore);
  }

  void Update()
  {
    if (!dead)
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
    player = Instantiate(playerPrefab, playerSpawnPosition, Quaternion.identity);
    cameraController.player = player;
  }

  public void SpawnBox()
  {
    List<Vector2Int> possibleSpawnTiles = LevelManager.level.boxSpawnPoints;
    Vector3 boxSpawnPosition;
    do
    {
      Vector2Int chosenSpawnTile = Util.RandomFromList(possibleSpawnTiles);
      boxSpawnPosition = Util.WorldPositionFromTile(chosenSpawnTile);
    } while (Vector3.Distance(boxSpawnPosition, player.transform.position) < minBoxSpawnDistanceToPlayer);

    box = Instantiate(boxPrefab, boxSpawnPosition, Quaternion.identity);
  }

  public void HitStop(float duration, Action callback = null)
  {
    if (hitStopRoutine != null)
    {
      StopCoroutine(hitStopRoutine);
    }
    float previousTimescale = Time.timeScale;
    hitStopRoutine = StartCoroutine(HitStopRoutine(duration, previousTimescale, callback));
  }

  private IEnumerator HitStopRoutine(float duration, float previousTimescale, Action callback)
  {
    Time.timeScale = 0;
    yield return new WaitForSecondsRealtime(duration);
    Time.timeScale = previousTimescale;
    if (callback is not null) { 
      callback();
    }
    hitStopRoutine = null;
  }

  public void Restart()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }

  public void IncrementBoxScore()
  {
    boxScore++;
    if (boxScore > PersistentData.BestBoxScore)
    {
      PersistentData.BestBoxScore = boxScore;
    }
    UIController.instance.UpdateScoreLabels(boxScore, PersistentData.BestBoxScore);
  }

  public void CreateFloatingText(Vector3 floatingTextPosition, string text)
  {
    GameObject floatingText = GameObject.Instantiate(floatingTextPrefab, floatingTextPosition, Quaternion.identity);
    floatingText.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(text);
  }
}
