using UnityEngine;

public class Bazooka : AbstractSemiAutoGun
{
  public override string GunName { get; } = "Bazooka";
  protected override string BulletPrefabPath { get; } = "Prefabs/Bullets/Rocket";
  protected override string MuzzleFlashPrefabPath { get; } = "Prefabs/Bullet Muzzle Flash";
  [field: SerializeField]
  protected override float Cooldown { get; set; } = 1f;
  [field: SerializeField]
  protected override float RandomSpread { get; set; } = 0;
}