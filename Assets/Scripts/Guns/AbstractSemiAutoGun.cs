using UnityEngine;

public abstract class AbstractSemiAutoGun : AbstractGun
{
  public override void OnFirePush()
  {
    if (cantFireUntil < Time.time)
    {
      createBulletsTowardsCursor();
      cantFireUntil = Time.time + Cooldown;
    }
  }
}
