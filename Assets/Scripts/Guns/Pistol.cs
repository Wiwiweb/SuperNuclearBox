using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pistol : AbstractGun
{

  public float cooldown = 0.1f;

  public GameObject bulletPrefab;
  private float cantFireUntil = 0;

  new void Start()
  {
    bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet");
    base.Start();
  }

  void Update()
  {

  }

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
