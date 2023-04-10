using UnityEngine;

public class Shotgun : AbstractGun
{
  public override string GunName { get => "Shotgun"; }
  protected override string GunSpritePath { get => "Weapon sprites/Shotgun"; }
  protected override string BulletPrefabPath { get => "Prefabs/Bullets/Shell"; }
  protected override string MuzzleFlashPrefabPath { get => "Prefabs/Bullet Muzzle Flash"; }
  protected override float Cooldown { get => 1.0f; }
  protected override float Spread { get => 30f; }

  private int nbShells = 8;

  public override void OnFirePush()
  {
    if (cantFireUntil < Time.time)
    {
      for (int i = 0; i < nbShells; i++)
      {
        createBulletTowardsCursor(bulletPrefab);
        cantFireUntil = Time.time + Cooldown;
      }
    }
  }
}
