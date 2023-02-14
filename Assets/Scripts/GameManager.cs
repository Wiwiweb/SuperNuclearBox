using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{

  [SerializeField]
  private GameObject playerPrefab;
  [SerializeField]
  private GameObject boxPrefab;

  private GameObject player;
  private FollowPlayer cameraScript;
  private Tilemap wallTilemap;

  void Start()
  {
    cameraScript = GameObject.Find("Main Camera").GetComponent<FollowPlayer>();
    wallTilemap = GameObject.Find("Tilemap_Walls").GetComponent<Tilemap>();
    LevelManager.CreateLevel();
    spawnPlayer();
    spawnBox();
  }

  void Update()
  {
    if (Keyboard.current.rKey.wasPressedThisFrame)
    {
      LevelManager.DestroyLevel();
      Destroy(player);
      LevelManager.CreateLevel();
      spawnPlayer();
      spawnBox();
    }

  }

  private void spawnPlayer()
  {
    Vector2Int spawnPointTile = LevelManager.level.spawnPointTile;
    Debug.Log($"Player spawn tile: {spawnPointTile}");
    Vector3 playerSpawnPosition = wallTilemap.GetCellCenterWorld(new Vector3Int(spawnPointTile.x, spawnPointTile.y, 1));
    playerSpawnPosition.z = 1;
    player = Instantiate(playerPrefab, playerSpawnPosition, Quaternion.identity);
    cameraScript.player = player;
  }

  public void spawnBox()
  {
    List<Vector2Int> possibleSpawnTiles = LevelManager.level.boxSpawnPoints;
    int chosenSpawnTileIndex = Random.Range(0, possibleSpawnTiles.Count);
    Vector2Int chosenSpawnTile = possibleSpawnTiles[chosenSpawnTileIndex];
    Debug.Log($"Box spawn tile: {chosenSpawnTile}");
    Vector3 boxSpawnPosition = wallTilemap.GetCellCenterWorld(new Vector3Int(chosenSpawnTile.x, chosenSpawnTile.y, 1));
    boxSpawnPosition.z = 1;
    Instantiate(boxPrefab, boxSpawnPosition, Quaternion.identity);
  }
}
