using UnityEngine;

public class LaserGun : AbstractSemiAutoGun
{
  public override string GunName { get; } = "Laser Gun";
  protected override string BulletPrefabPath { get; } = "Prefabs/Bullets/Laser";
  [field: SerializeField]
  protected override float Cooldown { get; set; } = 0.11f;
  [field: SerializeField]
  protected override float RandomSpread { get; set; } = 5;
  [field: SerializeField]
  protected override float CameraKickback { get; set; } = 0;

}
