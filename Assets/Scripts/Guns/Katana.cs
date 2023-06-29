using UnityEngine;

public class Katana : AbstractSemiAutoGun
{
  public override string GunName { get; } = "Katana";
  protected override string BulletPrefabPath { get; } = "Prefabs/Bullets/MeleeSlash";
  public override string GunPickupSoundPath { get; } = "Gun pickup sounds/Katana";
  [field: SerializeField]
  protected override float Cooldown { get; set; } = 0.11f;
  [field: SerializeField]
  protected override float RandomSpread { get; set; } = 10;
  [field: SerializeField]
  protected override float HeldAngle { get; set; } = 90;
  [field: SerializeField]
  protected override Vector2 HeldOffset { get; set; } = new Vector2(-0.10f, 0);
  [field: SerializeField]
  protected override float BulletSpawnOffset { get; set; } = 0.4f;
}
