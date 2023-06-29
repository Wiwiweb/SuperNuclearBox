using UnityEngine;

public class Peashooter : Pistol
{
  public override string GunName { get; } = "Peashooter";
  protected override string BulletPrefabPath { get; } = "Prefabs/Bullets/Shell";
  protected override float RandomSpread { get; set; } = 30;
  public override GunRarityType GunRarity { get; set; } = GunRarityType.Bad;
}
