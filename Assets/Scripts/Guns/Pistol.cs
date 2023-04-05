using UnityEngine;

public class Pistol : AbstractSemiAutoGun
{
  public override string gunName
  {
    get
    { return "Pistol"; }
  }

  new void Start()
  {
    cooldown = 0.18f;
    spread = 15;
    bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullets/Bullet");
    muzzleFlashPrefab = Resources.Load<GameObject>("Prefabs/Bullet Muzzle Flash");
    gunSprite = Resources.Load<Sprite>("Weapon sprites/Pistol");
    base.Start();
  }
}
