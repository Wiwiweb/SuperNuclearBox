public class TripleMachineGun : AbstractAutomaticGun
{
  public override string GunName { get => "Triple machine gun"; }
  protected override string GunSpritePath { get => "Weapon sprites/Triple machine gun"; }
  protected override string BulletPrefabPath { get => "Prefabs/Bullets/Bullet"; }
  protected override string MuzzleFlashPrefabPath { get => "Prefabs/Bullet Muzzle Flash"; }
  protected override float Cooldown { get => 0.18f; }
  protected override float RandomSpread { get => 30f; }
  protected override float FixedSpread { get => 30f; }
  protected override int NbProjectiles { get => 3; }
}
