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

  public static Vector2 RoundToPixel(Vector2 vector)
  {
    return new Vector2(
      Mathf.Round(vector.x * PixelsPerUnit) / PixelsPerUnit,
      Mathf.Round(vector.y * PixelsPerUnit) / PixelsPerUnit
    );
  }

  public static void printStopwatch(Stopwatch stopwatch)
  {
    stopwatch.Stop();
    Debug.Log($"Stopwatch: {stopwatch.ElapsedMilliseconds}");
    stopwatch.Reset();
    stopwatch.Start();
  }
}
