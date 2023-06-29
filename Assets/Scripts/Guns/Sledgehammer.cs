using UnityEngine;

public class Sledgehammer : AbstractSemiAutoGun
{
  public override string GunName { get; } = "Sledgehammer";
  protected override string BulletPrefabPath { get; } = "Prefabs/Bullets/MeleeBigSlash";
  public override string GunPickupSoundPath { get; } = "Gun pickup sounds/Sledgehammer";
  [field: SerializeField]
  protected override float Cooldown { get; set; } = 1.0f;
  [field: SerializeField]
  protected override float RandomSpread { get; set; } = 10;
  [field: SerializeField]
  protected override float HeldAngle { get; set; } = 90;
  [field: SerializeField]
  protected override Vector2 HeldOffset { get; set; } = new Vector2(-0.10f, 0);
  [field: SerializeField]
  protected override float BulletSpawnOffset { get; set; } = 0.4f;
}
