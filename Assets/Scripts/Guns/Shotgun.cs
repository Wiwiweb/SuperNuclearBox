using UnityEngine;

public class Shotgun : AbstractSemiAutoGun
{
  public override string GunName { get; } = "Shotgun";
  protected override string BulletPrefabPath { get; } = "Prefabs/Bullets/Shell";
  protected override string MuzzleFlashPrefabPath { get; } = "Prefabs/Bullet Muzzle Flash";
  [field: SerializeField]
  protected override float Cooldown { get; set; } = 1.0f;
  [field: SerializeField]
  protected override float RandomSpread { get; set; } = 30;
  [field: SerializeField]
  protected override int NbProjectiles { get; set; } = 8;
  [field: SerializeField]
  protected override float CameraKickback { get; set; } = 5;
}
