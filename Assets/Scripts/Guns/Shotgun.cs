public class Shotgun : AbstractSemiAutoGun
{
  public override string GunName { get => "Shotgun"; }
  protected override string GunSpritePath { get => "Weapon sprites/Shotgun"; }
  protected override string BulletPrefabPath { get => "Prefabs/Bullets/Shell"; }
  protected override string MuzzleFlashPrefabPath { get => "Prefabs/Bullet Muzzle Flash"; }
  protected override float Cooldown { get => 1.0f; }
  protected override float Spread { get => 30f; }
  protected override int NbProjectiles { get => 8; }
}
