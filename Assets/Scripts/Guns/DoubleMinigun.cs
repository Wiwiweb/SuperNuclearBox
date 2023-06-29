using UnityEngine;

public class DoubleMinigun : Minigun
{
  public override string GunName { get; } = "Double Minigun";
  [field: SerializeField]
  protected override float Cooldown { get; set; } = 0.025f;
  [field: SerializeField]
  protected override float RandomSpread { get; set; } = 60;
  [field: SerializeField]
  protected override float Recoil { get; set; } = 4;
}
