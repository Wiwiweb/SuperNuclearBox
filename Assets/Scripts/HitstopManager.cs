using System;
using System.Collections.Generic;
using UnityEngine;

public class HitstopManager : MonoBehaviour
{
  public static HitstopManager instance;

  private float normalTimeScale = 1;
  private bool inHitstop = false;
  private float endHitstopAt;
  private List<Action> callbacks = new List<Action>();

  void Awake()
  {
    instance = this;
  }

  void Update()
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

  public void AddHitstop(float addedTime, Action callback = null)
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
}
