using UnityEngine;

public abstract class AbstractSemiAutoGun : AbstractGun
{
  public override void onFirePush()
  {
    if (cantFireUntil < Time.time)
    {
      createBulletTowardsCursor(bulletPrefab);
      cantFireUntil = Time.time + cooldown;
    }
  }

  public override void onFireStop()
  {

  }
}
