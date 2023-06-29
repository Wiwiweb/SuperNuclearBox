using UnityEngine;

public class SuperBazooka : Bazooka
{
  public override string GunName { get; } = "Super Bazooka";
  [field: SerializeField]
  protected override float Cooldown { get; set; } = 1.5f;
  [field: SerializeField]
  protected override float FixedSpread { get; set; } = 25;
  [field: SerializeField]
  protected override int NbProjectiles { get; set; } = 5;
  [field: SerializeField]
  protected override float CameraKickback { get; set; } = 5;
  public override GunRarityType GunRarity { get; set; } = GunRarityType.Good;
}