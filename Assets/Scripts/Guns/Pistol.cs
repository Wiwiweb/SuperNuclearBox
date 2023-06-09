using UnityEngine;

public class Pistol : AbstractSemiAutoGun
{
  public override string GunName { get; } = "Pistol";
  protected override string BulletPrefabPath { get; } = "Prefabs/Bullets/Bullet";
  protected override string MuzzleFlashPrefabPath { get; } = "Prefabs/Bullet Muzzle Flash";
  [field: SerializeField]
  protected override float Cooldown { get; set; } = 0.11f;
  [field: SerializeField]
  protected override float RandomSpread { get; set; } = 10;
}
