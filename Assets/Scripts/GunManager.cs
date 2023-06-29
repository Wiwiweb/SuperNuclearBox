using System;
using System.Collections.Generic;

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

  private static List<GunEntry> allGuns = new List<GunEntry>()
  {
    new GunEntry(10, typeof(Pistol)),
    new GunEntry(10, typeof(MachineGun)),
    new GunEntry(10, typeof(Shotgun)),
    new GunEntry(10, typeof(LaserGun)),
    new GunEntry(5, typeof(TripleMachineGun)),
    new GunEntry(5, typeof(DiscGun)),
    new GunEntry(5, typeof(LaserRifle)),
    new GunEntry(5, typeof(Bazooka)),
    new GunEntry(5, typeof(GrenadeLauncher)),
    new GunEntry(5, typeof(Katana)),
    new GunEntry(5, typeof(Sledgehammer)),
    new GunEntry(2, typeof(Minigun)),
    new GunEntry(2, typeof(Screwdriver)),
    new GunEntry(2, typeof(GoldenGun)),
    new GunEntry(2, typeof(Peashooter)),
    new GunEntry(1, typeof(DoubleMinigun)),
    new GunEntry(1, typeof(LaserMinigun)),
    new GunEntry(1, typeof(QuadrupleMachineGun)),
    new GunEntry(1, typeof(SuperDiscGun)),
    new GunEntry(1, typeof(SuperBazooka)),
    new GunEntry(1, typeof(SuperGrenadeLauncher)),
  };
  public static List<Type> gunSpawnTable = new List<Type>();

  public static void Init()
  {
    foreach (GunEntry entry in allGuns)
    {
      for (int i = 0; i < entry.probabilityWeight; i++)
      {
        gunSpawnTable.Add(entry.type);
      }
    }
  }

  public static Type GetRandomGunType()
  {
    Type chosenGun;
    do
    {
      chosenGun = Util.RandomFromList(gunSpawnTable);
    } while (chosenGun == PersistentData.PlayerEquippedGunType);
    return chosenGun;
  }

  public static Type DebugGetPreviousGun()
  {
    int gunIndex = DebugGetGunIndex() - 1;
    if (gunIndex < 0) { gunIndex = allGuns.Count - 1; }
    return allGuns[gunIndex].type;
  }

  public static Type DebugGetNextGun()
  {
    int gunIndex = DebugGetGunIndex() + 1;
    if (gunIndex >= allGuns.Count) { gunIndex = 0; }
    return allGuns[gunIndex].type;
  }

  private static int DebugGetGunIndex()
  {
    for (int i = 0; i< allGuns.Count; i++)
    {
      if (allGuns[i].type == PersistentData.PlayerEquippedGunType){
        return i;
      }
    }
    return 0;
  }
}
