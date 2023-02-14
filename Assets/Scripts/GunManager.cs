using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Tilemaps;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public static class GunManager
{
  private static Type[] allGuns = {
    typeof(Pistol),
    typeof(MachineGun),
  };

  public static AbstractGun getRandomGun()
  {
    int gunIndex = Random.Range(0, allGuns.GetUpperBound(0));
    Type gunType = allGuns[gunIndex];
    return (AbstractGun) Activator.CreateInstance(gunType);
  }
}
