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

  public static Vector2 CollisionBounce(Collision2D collision, Transform transform, Vector2 movementDirection)
  {
    ContactPoint2D[] contactPoints = new ContactPoint2D[collision.contactCount];
    collision.GetContacts(contactPoints);

    Vector2 averageContactPoint = Vector2.zero;
    foreach (ContactPoint2D contactPoint in contactPoints)
    {
      // Instantiate(positionMarkerPrefab, contactPoint.point, Quaternion.identity);
      averageContactPoint += contactPoint.point;
    }
    averageContactPoint /= contactPoints.Length;

    Vector2 localContactPoint = transform.InverseTransformPoint(averageContactPoint);
    localContactPoint *= transform.localScale;
    if (Math.Abs(localContactPoint.x) <= Math.Abs(localContactPoint.y)) // Horizontal wall
    {
      if (localContactPoint.y > 0) // Top hit
      {
        movementDirection.y = -Math.Abs(movementDirection.y);
      }
      else // Bottom hit
      {
        movementDirection.y = Math.Abs(movementDirection.y);
      }
    }
    if (Math.Abs(localContactPoint.x) >= Math.Abs(localContactPoint.y)) // Vertical wall (not in an else, to allow for corner hits when x == y)
    {
      if (localContactPoint.x > 0) // Right hit
      {
        movementDirection.x = -Math.Abs(movementDirection.x);
        transform.localScale = new Vector3(-1, 1, 1);
      }
      else // Left hit
      {
        movementDirection.x = Math.Abs(movementDirection.x);
        transform.localScale = new Vector3(1, 1, 1);
      }
    }

    return movementDirection;
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
    for (int y = charArray.GetUpperBound(1); y >= 0; y--) {
      for (int x = 0; x <= charArray.GetUpperBound(0); x++) {
            s += charArray[x,y];
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
