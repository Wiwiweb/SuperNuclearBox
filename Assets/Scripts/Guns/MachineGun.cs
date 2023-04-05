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
    cooldown = 0.18f;
    spread = 30;
    bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullets/Bullet");
    muzzleFlashPrefab = Resources.Load<GameObject>("Prefabs/Bullet Muzzle Flash");
    gunSprite = Resources.Load<Sprite>("Weapon sprites/Machine gun");
    base.Start();
  }
}
