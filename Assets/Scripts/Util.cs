using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

internal static class Util
{
  public static int PixelsPerUnit = 100;

  public static Quaternion RoundRotation(Quaternion rotation, int angle = 1)
  {
    Vector3 eulerRotation = rotation.eulerAngles;
    eulerRotation = new Vector3(
      Mathf.Round(eulerRotation.x / angle) * angle,
      Mathf.Round(eulerRotation.y / angle) * angle,
      Mathf.Round(eulerRotation.z / angle) * angle);
    return Quaternion.Euler(eulerRotation);
  }

  public static (Vector2, Vector2) RoundToPixel(Vector2 vector)
  {
    Vector2 roundedVector = new Vector2(
      Mathf.Round(vector.x * PixelsPerUnit) / PixelsPerUnit,
      Mathf.Round(vector.y * PixelsPerUnit) / PixelsPerUnit
    );
    return (roundedVector, roundedVector - vector);
  }

  public static void PrintStopwatch(Stopwatch stopwatch)
  {
    stopwatch.Stop();
    Debug.Log($"Stopwatch: {stopwatch.ElapsedMilliseconds}");
    stopwatch.Reset();
    stopwatch.Start();
  }

  public static String CharArrayToString(Char[,] charArray)
  {
    var s = "";
    for (int y = charArray.GetUpperBound(1); y >= 0; y--)
    {
      for (int x = 0; x <= charArray.GetUpperBound(0); x++)
      {
        s += charArray[x, y];
      }
      s += '\n';
    }
    return s;
  }

  public static Vector3 WorldPositionFromTile(Vector2Int tile)
  {
    Vector3 worldPosition = GameManager.instance.wallTilemap.GetCellCenterWorld(new Vector3Int(tile.x, tile.y, 1));
    worldPosition.z = 1;
    return worldPosition;
  }

  public static T RandomFromList<T>(List<T> list)
  {
    int chosenIndex = Random.Range(0, list.Count);
    return list[chosenIndex];
  }

  public static Quaternion GetRandomAngle()
  {
    float randomAngle = Random.Range(0f, 360f);
    return Quaternion.Euler(0, 0, randomAngle);
  }

  public static Vector2 GetRandomAngleVector()
  {
    float randomAngle = Random.Range(0f, 360f);
    return new Vector2((float)Math.Cos(randomAngle), (float)Math.Sin(randomAngle));
  }
}
