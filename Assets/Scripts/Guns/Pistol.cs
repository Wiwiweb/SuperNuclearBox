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
    cooldown = 0.3f;
    spread = 15;
    bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet");
    gunSprite = Resources.Load<Sprite>("Weapon sprites/Pistol");
    base.Start();
  }
}
