using UnityEngine;

public class DiscGun : AbstractSemiAutoGun
{
  public override string GunName { get; } = "Disc Gun";
  protected override string BulletPrefabPath { get; } = "Prefabs/Bullets/Disc";
  protected override string MuzzleFlashPrefabPath { get; } = "Prefabs/Bullet Muzzle Flash";
  [field: SerializeField]
  protected override float Cooldown { get; set; } = 0.8f;
  [field: SerializeField]
  protected override float RandomSpread { get; set; } = 5;
  [field: SerializeField]
  protected override float CameraKickback { get; set; } = 1;
}
