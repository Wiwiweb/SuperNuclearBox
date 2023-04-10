using UnityEngine;

public abstract class AbstractSemiAutoGun : AbstractGun
{
  public override void OnFirePush()
  {
    if (cantFireUntil < Time.time)
    {
      createBulletTowardsCursor(bulletPrefab);
      cantFireUntil = Time.time + Cooldown;
    }
  }
}
