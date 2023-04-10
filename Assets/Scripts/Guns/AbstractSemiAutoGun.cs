using UnityEngine;

public abstract class AbstractSemiAutoGun : AbstractGun
{
  public override void OnFirePush()
  {
    if (cantFireUntil < Time.time)
    {
      createNBulletsTowardsCursor();
      cantFireUntil = Time.time + Cooldown;
    }
  }
}
