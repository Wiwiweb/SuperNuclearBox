public class Pistol : AbstractSemiAutoGun
{
  public override string GunName { get => "Pistol"; }
  protected override string GunSpritePath { get => "Weapon sprites/Pistol"; }
  protected override string BulletPrefabPath { get => "Prefabs/Bullets/Bullet"; }
  protected override string MuzzleFlashPrefabPath { get => "Prefabs/Bullet Muzzle Flash"; }
  protected override float Cooldown { get => 0.18f; }
  protected override float RandomSpread { get => 15; }
}
