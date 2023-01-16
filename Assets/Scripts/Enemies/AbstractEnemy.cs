using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractEnemy : MonoBehaviour
{
  public float health;

  public void onBulletHit(GameObject bullet)
  {
    health -= bullet.GetComponent<BulletController>().damage;
    if (health <= 0)
    {
      Destroy(gameObject);
    }
  }
}
