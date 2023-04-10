using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

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
  private float minBoxSpawnDistanceToPlayer = 2;


  private CameraController cameraScript;
  private Coroutine hitStopRoutine;
  private int boxScore = 0;


  void Awake() {
      instance = this;
  }

  void Start()
  {
    cameraScript = Camera.main.GetComponent<CameraController>();

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
    if (Keyboard.current.rKey.wasPressedThisFrame)
    {
      SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
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
    cameraScript.player = player;
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
    hitStopRoutine = StartCoroutine(HitStopRoutine(duration, callback));
  }

  private IEnumerator HitStopRoutine(float duration, Action callback)
  {
    Time.timeScale = 0;
    yield return new WaitForSecondsRealtime(duration);
    Time.timeScale = 1;
    if (callback is not null) { 
      callback();
    }
    hitStopRoutine = null;
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
}
