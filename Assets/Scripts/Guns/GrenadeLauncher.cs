using UnityEngine;

public class GrenadeLauncher : AbstractSemiAutoGun
{
  public override string GunName { get; } = "Grenade Launcher";
  protected override string BulletPrefabPath { get; } = "Prefabs/Bullets/Grenade";
  protected override string MuzzleFlashPrefabPath { get; } = "Prefabs/Bullet Muzzle Flash";
  [field: SerializeField]
  protected override float Cooldown { get; set; } = 0.8f;
  [field: SerializeField]
  protected override float RandomSpread { get; set; } = 0;
}
