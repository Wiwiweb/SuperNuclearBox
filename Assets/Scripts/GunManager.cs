using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class GunManager
{
  public class GunEntry
  {
    public GunEntry(int probabilityWeight, Type type)
    {
      this.probabilityWeight = probabilityWeight;
      this.type = type;
    }

    public int probabilityWeight;
    public Type type;
  }

  public static List<Type> gunSpawnTable = new List<Type>();

  public static void Init()
  {
    List<GunEntry> allGuns = new List<GunEntry>();
    allGuns.Add(new GunEntry(10, typeof(Pistol)));
    allGuns.Add(new GunEntry(10, typeof(MachineGun)));
    allGuns.Add(new GunEntry(10, typeof(Shotgun)));

    foreach (GunEntry entry in allGuns)
    {
      for (int i = 0; i < entry.probabilityWeight; i++)
      {
        gunSpawnTable.Add(entry.type);
      }
    }
  }

  public static Type getRandomGunType()
  {
    Type chosenGun;
    Type currentGun = GameManager.instance.player.GetComponent<PlayerController>().equippedGun.GetType();
    do
    {
      chosenGun = Util.RandomFromList(gunSpawnTable);
    } while (chosenGun == currentGun);
    return chosenGun;
  }
}
