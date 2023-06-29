using UnityEngine;

public class GoldenGun : Pistol
{
  public override string GunName { get; } = "Golden Gun";
  protected override string BulletPrefabPath { get; } = "Prefabs/Bullets/Gold Bullet";
  public override string GunPickupSoundPath { get; } = "Gun pickup sounds/Gold";
  public override GunRarityType GunRarity { get; set; } = GunRarityType.Good;
}
