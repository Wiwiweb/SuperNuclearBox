using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

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

  public static void printStopwatch(Stopwatch stopwatch)
  {
    stopwatch.Stop();
    Debug.Log($"Stopwatch: {stopwatch.ElapsedMilliseconds}");
    stopwatch.Reset();
    stopwatch.Start();
  }
}
