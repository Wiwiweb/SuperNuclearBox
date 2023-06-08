using UnityEngine;

public class LimitedRangeBulletController : BulletController
{
  [SerializeField]
  private float speedDecrease;
  [SerializeField]
  private float destroyAtSpeed;

  new void FixedUpdate()
  {
    speed /= speedDecrease;
    if (speed <= destroyAtSpeed)
    {
      Destroy(gameObject);
    }
    else
    {
      base.FixedUpdate();
    }
  }
}
