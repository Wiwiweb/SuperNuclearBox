using UnityEngine;

public class MachineGun : AbstractAutomaticGun
{
  public override string GunName { get; } = "Machine gun";
  protected override string GunSpritePath { get; } = "Weapon sprites/Machine gun";
  protected override string BulletPrefabPath { get; } = "Prefabs/Bullets/Bullet";
  protected override string MuzzleFlashPrefabPath { get; } = "Prefabs/Bullet Muzzle Flash";
  [field: SerializeField]
  protected override float Cooldown { get; set; } = 0.18f;
  [field: SerializeField]
  protected override float RandomSpread { get; set; } = 20;
}
