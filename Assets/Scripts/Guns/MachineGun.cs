using UnityEngine;

public class MachineGun : AbstractAutomaticGun
{
  // Start is called before the first frame update
  new void Start()
  {
    cooldown = 0.2f;
    spread = 30;
    bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet");
    gunSprite = Resources.Load<Sprite>("Weapon sprites/Machine gun");
    base.Start();
  }
}
