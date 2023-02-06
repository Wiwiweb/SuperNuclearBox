using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pistol : AbstractSemiAutoGun
{
  new void Start()
  {
    cooldown = 0.3f;
    spread = 15;
    bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet");
    gunSprite = Resources.Load<Sprite>("Weapon sprites/Pistol");
    base.Start();
  }
}
