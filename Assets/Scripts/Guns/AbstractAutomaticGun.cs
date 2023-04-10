using UnityEngine;

public abstract class AbstractAutomaticGun : AbstractGun
{
  private bool isFiring = false;

  void Update()
  {
    if (isFiring && cantFireUntil < Time.time)
    {
      createBulletTowardsCursor(bulletPrefab);
      cantFireUntil = Time.time + Cooldown;
    }
  }

  public override void OnFirePush()
  {
    isFiring = true;
  }

  public override void OnFireStop()
  {
    isFiring = false;
  }
}
