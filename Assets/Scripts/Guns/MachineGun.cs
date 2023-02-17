using UnityEngine;

public class MachineGun : AbstractAutomaticGun
{
  public override string gunName
  {
    get
    { return "Machine gun"; }
  }

  new void Start()
  {
    cooldown = 0.2f;
    spread = 30;
    bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet");
    gunSprite = Resources.Load<Sprite>("Weapon sprites/Machine gun");
    base.Start();
  }
}
