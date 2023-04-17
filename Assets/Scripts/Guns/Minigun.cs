using UnityEngine;

public class Minigun : AbstractAutomaticGun
{
  public override string GunName { get; } = "Minigun";
  protected override string GunSpritePath { get; } = "Weapon sprites/Minigun";
  protected override string BulletPrefabPath { get; } = "Prefabs/Bullets/Bullet";
  protected override string MuzzleFlashPrefabPath { get; } = "Prefabs/Bullet Muzzle Flash";
  [field: SerializeField]
  protected override float Cooldown { get; set; } = 0.05f;
  [field: SerializeField]
  protected override float RandomSpread { get; set; } = 40;
  [field: SerializeField]
  protected override float Recoil { get; set; } = 2;
}
