public class Minigun : AbstractAutomaticGun
{
  public override string GunName { get => "Minigun"; }
  protected override string GunSpritePath { get => "Weapon sprites/Minigun"; }
  protected override string BulletPrefabPath { get => "Prefabs/Bullets/Bullet"; }
  protected override string MuzzleFlashPrefabPath { get => "Prefabs/Bullet Muzzle Flash"; }
  protected override float Cooldown { get => 0.05f; }
  protected override float RandomSpread { get => 40; }
  protected override float Recoil { get => 2; }
}
