using System.Collections.Generic;
using UnityEngine;

public static class EnemySpawnManager
{
  public class EnemySpawnEntry
  {
    public EnemySpawnEntry(int probabilityWeight, int points, GameObject prefab)
    {
      this.probabilityWeight = probabilityWeight;
      this.points = points;
      this.prefab = prefab;
    }

    public int probabilityWeight;
    public int points;
    public GameObject prefab;
  }

  public static List<EnemySpawnEntry> enemySpawnTable = new List<EnemySpawnEntry>();

  private static float minSpawnDistanceToPlayer = 2;
  private static float enemyPointsAtStart = 3;
  private static float enemyPointGainPerSec = 1;

  private static float enemyPoints;

  private static GameObject GetEnemyPrefab(string name)
  {
    return Resources.Load<GameObject>("Prefabs/Enemies/" + name);
  }

  public static void Init()
  {
    List<EnemySpawnEntry> allEnemies = new List<EnemySpawnEntry>();
    allEnemies.Add(new EnemySpawnEntry(3, 3, GetEnemyPrefab("SCB_Zombie")));
    allEnemies.Add(new EnemySpawnEntry(1, 1, GetEnemyPrefab("SCB_Fattie")));
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
    enemyPoints = enemyPointsAtStart;
  }

  public static void UpdateEnemySpawns()
  {
    enemyPoints += enemyPointGainPerSec * Time.deltaTime;
    if (enemyPoints >= 0)
    {
      EnemySpawnEntry enemy = getRandomEnemy();
      Vector3 spawnPosition;
      do
      {
        Vector2Int spawnPointTile = getRandomSpawnPosition();
        spawnPosition = Util.WorldPositionFromTile(spawnPointTile);
      } while (Vector3.Distance(spawnPosition, GameManager.instance.player.transform.position) < minSpawnDistanceToPlayer);
      Object.Instantiate(enemy.prefab, spawnPosition, Quaternion.identity);
      enemyPoints -= enemy.points;
    }
  }

  public static EnemySpawnEntry getRandomEnemy()
  {
    return Util.RandomFromList(enemySpawnTable);
  }

  public static Vector2Int getRandomSpawnPosition()
  {
    return Util.RandomFromList(LevelManager.level.enemySpawnPoints);
  }
}
