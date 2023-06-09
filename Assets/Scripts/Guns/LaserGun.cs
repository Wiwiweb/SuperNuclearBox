using UnityEngine;

public class LaserGun : AbstractSemiAutoGun
{
  public override string GunName { get; } = "Laser Gun";
  protected override string BulletPrefabPath { get; } = "Prefabs/Bullets/Laser";
  public override string GunPickupSoundPath { get; } = "Gun pickup sounds/Energy";
  [field: SerializeField]
  protected override float Cooldown { get; set; } = 0.11f;
  [field: SerializeField]
  protected override float RandomSpread { get; set; } = 5;
  [field: SerializeField]
  protected override float CameraKickback { get; set; } = 0;
  [field: SerializeField]
  protected override float BulletSpawnOffset { get; set; } = -0.02f;

}
