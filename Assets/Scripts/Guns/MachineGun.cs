using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MachineGun : AbstractGun
{
  public float cooldown = 0.2f;
  private bool isFiring = false;


  // Start is called before the first frame update
  new void Start()
  {
    bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet");
    gunSprite = Resources.Load<Sprite>("Weapon sprites/Machine gun");
    base.Start();
  }

  // Update is called once per frame
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
