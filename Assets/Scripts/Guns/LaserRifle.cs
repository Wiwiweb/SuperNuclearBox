using UnityEngine;

public class LaserRifle : AbstractAutomaticGun
{
  public override string GunName { get; } = "Laser Rifle";
  protected override string BulletPrefabPath { get; } = "Prefabs/Bullets/Laser";
  public override string GunPickupSoundPath { get; } = "Gun pickup sounds/Energy";
  protected override string GunSoundPath { get; } = "Gun sounds/LaserGun";
  [field: SerializeField]
  protected override float Cooldown { get; set; } = 0.18f;
  [field: SerializeField]
  protected override float RandomSpread { get; set; } = 20;
  [field: SerializeField]
  protected override float CameraKickback { get; set; } = 0;
  [field: SerializeField]
  protected override float BulletSpawnOffset { get; set; } = -0.02f;
}
