using UnityEngine;

public class SuperGrenadeLauncher : GrenadeLauncher
{
  public override string GunName { get; } = "Super Grenade Launcher";
  [field: SerializeField]
  protected override float Cooldown { get; set; } = 1.2f;
  [field: SerializeField]
  protected override float FixedSpread { get; set; } = 40;
  [field: SerializeField]
  protected override int NbProjectiles { get; set; } = 5;
  [field: SerializeField]
  protected override float CameraKickback { get; set; } = 3;
  public override GunRarityType GunRarity { get; set; } = GunRarityType.Good;
}