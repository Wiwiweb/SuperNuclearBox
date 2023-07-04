using System;
using System.Collections.Generic;
using UnityEngine;

public static class Hitstop
{
  private static float normalTimeScale = 1;
  private static bool inHitstop = false;
  private static float endHitstopAt;
  private static List<Action> callbacks = new List<Action>();

  public static void Update()
  {
    if (inHitstop && Time.realtimeSinceStartup >= endHitstopAt)
    {
      inHitstop = false;
      Time.timeScale = normalTimeScale;
      foreach (Action callback in callbacks)
      {
        callback();
      }
      callbacks = new List<Action>();
    }
  }

  public static void Add(float addedTime, Action callback = null)
  {
    if (!inHitstop)
    {
      inHitstop = true;
      normalTimeScale = Time.timeScale;
      Time.timeScale = 0;
      endHitstopAt = Time.realtimeSinceStartup;
    }
    endHitstopAt += addedTime;
    if (callback is not null)
    {
      callbacks.Add(callback);
    }
  }

  public static void ClearCallbacks()
  {
    callbacks.Clear();
  }
}
