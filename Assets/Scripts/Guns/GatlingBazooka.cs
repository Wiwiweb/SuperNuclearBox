using UnityEngine;

public class GatlingBazooka : AbstractAutomaticGun
{
  public override string GunName { get; } = "Gatling Bazooka";
  protected override string BulletPrefabPath { get; } = "Prefabs/Bullets/Rocket";
  protected override string MuzzleFlashPrefabPath { get; } = "Prefabs/Bullet Muzzle Flash";
  protected override string GunSoundPath { get; } = "Gun sounds/Bazooka";
  [field: SerializeField]
  protected override float Cooldown { get; set; } = 0.2f;
  [field: SerializeField]
  protected override float RandomSpread { get; set; } = 10;
}