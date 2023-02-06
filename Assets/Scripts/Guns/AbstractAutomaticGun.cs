using UnityEngine;

public abstract class AbstractAutomaticGun : AbstractGun
{
  private bool isFiring = false;

  void Update()
  {
    if (isFiring && cantFireUntil < Time.time)
    {
      createBulletTowardsCursor(bulletPrefab);
      cantFireUntil = Time.time + cooldown;
    }
  }

  public override void onFirePush()
  {
    isFiring = true;
  }

  public override void onFireStop()
  {
    isFiring = false;
  }
}
