using UnityEngine;

public class Shotgun : AbstractGun
{
  private int nbShells = 8;

  public override string gunName
  {
    get
    { return "Shotgun"; }
  }

  new void Start()
  {
    cooldown = 1.0f;
    spread = 30;
    bulletPrefab = Resources.Load<GameObject>("Prefabs/LimitedRangeBullet");
    muzzleFlashPrefab = Resources.Load<GameObject>("Prefabs/Bullet Muzzle Flash");
    gunSprite = Resources.Load<Sprite>("Weapon sprites/Machine gun");
    base.Start();
  }

  public override void onFirePush()
  {
    if (cantFireUntil < Time.time)
    {
      for (int i = 0; i < nbShells; i++)
      {

        createBulletTowardsCursor(bulletPrefab);
        cantFireUntil = Time.time + cooldown;
      }
    }
  }

  public override void onFireStop()
  {

  }
}
