public class MachineGun : AbstractAutomaticGun
{
  public override string GunName { get => "Machine gun"; }
  protected override string GunSpritePath { get => "Weapon sprites/Machine gun"; }
  protected override string BulletPrefabPath { get => "Prefabs/Bullets/Bullet"; }
  protected override string MuzzleFlashPrefabPath { get => "Prefabs/Bullet Muzzle Flash"; }
  protected override float Cooldown { get => 0.18f; }
  protected override float RandomSpread { get => 30f; }
}
