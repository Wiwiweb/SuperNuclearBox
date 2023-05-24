using System.Collections.Generic;
using UnityEngine;

public static class EnemySpawnManager
{
  public enum SpawnType
  {
    Default,
    Big,
  }

  public class EnemySpawnEntry
  {
    public EnemySpawnEntry(int probabilityWeight, int points, GameObject prefab, SpawnType spawnType = SpawnType.Default)
    {
      this.probabilityWeight = probabilityWeight;
      this.points = points;
      this.prefab = prefab;
      this.spawnType = spawnType;
    }

    public int probabilityWeight;
    public int points;
    public GameObject prefab;
    public SpawnType spawnType;
  }

  public static List<EnemySpawnEntry> enemySpawnTable = new List<EnemySpawnEntry>();

  private const float MinSpawnDistanceToPlayer = 2;
  private const float EnemyPointsAtStart = 3;
  private const float EnemyPointGainPerSecBase = 1;
  private const float EnemyPointGainPerSecPerBox = 0.1f;
  private const int MaxSpawnTries = 10;

  private static float enemyPoints;

  private static GameObject GetEnemyPrefab(string name)
  {
    return Resources.Load<GameObject>("Prefabs/Enemies/" + name);
  }

  public static void Init()
  {
    List<EnemySpawnEntry> allEnemies = new List<EnemySpawnEntry>();
    allEnemies.Add(new EnemySpawnEntry(3, 3, GetEnemyPrefab("SCB_Zombie")));
    allEnemies.Add(new EnemySpawnEntry(1, 1, GetEnemyPrefab("SCB_Fattie"), SpawnType.Big));
    allEnemies.Add(new EnemySpawnEntry(2, 1, GetEnemyPrefab("SCB_FlameSkull")));

    foreach(EnemySpawnEntry entry in allEnemies)
    {
      for (int i = 0; i < entry.probabilityWeight; i++)
      {
        enemySpawnTable.Add(entry);
      }
    }

    ResetEnemyPoints();
  }

  public static void ResetEnemyPoints()
  {
    enemyPoints = EnemyPointsAtStart;
  }

  public static void UpdateEnemySpawns()
  {
    enemyPoints += (EnemyPointGainPerSecBase + EnemyPointGainPerSecPerBox * PersistentData.BoxScore) * Time.deltaTime;
    if (enemyPoints >= 0)
    {
      EnemySpawnEntry enemy = getRandomEnemy();
      Vector3 spawnPosition;
      int spawnTries = 0;
      do
      {
        Vector2Int spawnPointTile = getRandomSpawnPosition(enemy.spawnType);
        spawnPosition = Util.WorldPositionFromTile(spawnPointTile);
        spawnTries++;
        if (spawnTries > MaxSpawnTries)
        {
          return; // Give up this time, don't use up points
        }
      } while (Vector3.Distance(spawnPosition, GameManager.instance.Player.transform.position) < MinSpawnDistanceToPlayer);
      Object.Instantiate(enemy.prefab, spawnPosition, Quaternion.identity);
      enemyPoints -= enemy.points;
    }
  }

  public static EnemySpawnEntry getRandomEnemy()
  {
    return Util.RandomFromList(enemySpawnTable);
  }

  public static Vector2Int getRandomSpawnPosition(SpawnType spawnType)
  {
    switch(spawnType) 
    {
      case SpawnType.Big:
        return Util.RandomFromList(LevelManager.level.bigEnemySpawnPoints);
      default:
        return Util.RandomFromList(LevelManager.level.enemySpawnPoints);
    }
  }
}
