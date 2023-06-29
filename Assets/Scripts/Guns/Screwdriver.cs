using UnityEngine;

public class Screwdriver : AbstractSemiAutoGun
{
  public override string GunName { get; } = "Screwdriver";
  protected override string BulletPrefabPath { get; } = "Prefabs/Bullets/MeleeStab";
  public override string GunPickupSoundPath { get; } = "Gun pickup sounds/Katana";
  [field: SerializeField]
  protected override float Cooldown { get; set; } = 0.11f;
  [field: SerializeField]
  protected override float RandomSpread { get; set; } = 5;
  [field: SerializeField]
  protected override Vector2 HeldOffset { get; set; } = new Vector2(0, 0.08f);
  [field: SerializeField]
  protected override float BulletSpawnOffset { get; set; } = 0.2f;
}
