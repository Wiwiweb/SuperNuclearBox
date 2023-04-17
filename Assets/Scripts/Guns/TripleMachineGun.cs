using UnityEngine;

public class TripleMachineGun : AbstractAutomaticGun
{
  public override string GunName { get; } = "Triple machine gun";
  protected override string GunSpritePath { get; } = "Weapon sprites/Triple machine gun";
  protected override string BulletPrefabPath { get; } = "Prefabs/Bullets/Bullet";
  protected override string MuzzleFlashPrefabPath { get; } = "Prefabs/Bullet Muzzle Flash";
  [field: SerializeField]
  protected override float Cooldown { get; set; } = 0.18f;
  [field: SerializeField]
  protected override float RandomSpread { get; set; } = 30;
  [field: SerializeField]
  protected override float FixedSpread { get; set; } = 30;
  [field: SerializeField]
  protected override int NbProjectiles { get; set; } = 3;
}
