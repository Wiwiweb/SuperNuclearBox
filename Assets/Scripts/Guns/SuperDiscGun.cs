using UnityEngine;

public class SuperDiscGun : DiscGun
{
  public override string GunName { get; } = "Super Disc Gun";
  [field: SerializeField]
  protected override float RandomSpread { get; set; } = 0;
  [field: SerializeField]
  protected override float FixedSpread { get; set; } = 25;
  [field: SerializeField]
  protected override int NbProjectiles { get; set; } = 5;
  [field: SerializeField]
  protected override float CameraKickback { get; set; } = 3;
  public override GunRarityType GunRarity { get; set; } = GunRarityType.Bad;
}
