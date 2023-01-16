using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pistol : AbstractGun
{

  public float cooldown = 0.4f;

  public GameObject bulletPrefab;
  private float cantFireUntil = 0;

  // Start is called before the first frame update
  void Start()
  {
    bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet");
  }

  // Update is called once per frame
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
