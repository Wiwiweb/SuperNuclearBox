using System;
using Random = UnityEngine.Random;

public static class GunManager
{
  private static Type[] allGuns = {
    typeof(Pistol),
    // typeof(MachineGun),
    typeof(Shotgun),
  };

  public static Type getRandomGunType()
  {
    int gunIndex = Random.Range(0, allGuns.Length);
    return allGuns[gunIndex];
  }
}
