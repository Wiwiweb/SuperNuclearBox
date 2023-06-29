using UnityEngine;

public class QuadrupleMachineGun : TripleMachineGun
{
  public override string GunName { get; } = "Quadruple machine gun";
  [field: SerializeField]
  protected override int NbProjectiles { get; set; } = 4;
  [field: SerializeField]
  protected override float Recoil { get; set; } = 1.5f;
  public override GunRarityType GunRarity { get; set; } = GunRarityType.Good;
}
