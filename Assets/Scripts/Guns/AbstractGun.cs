using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class AbstractGun : MonoBehaviour
{
  public abstract void onFirePush();
  public abstract void onFireStop();

  protected void createBulletTowardsCursor(GameObject bulletPrefab)
  {
      Vector2 mousePosition = Mouse.current.position.ReadValue();
      Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();
      mousePosition = camera.ScreenToWorldPoint(mousePosition);

      Vector2 shootDirection = mousePosition - (Vector2)transform.position;
      shootDirection = shootDirection.normalized;
      GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.LookRotation(Vector3.forward, shootDirection));
      BulletController bulletScript = bullet.GetComponent<BulletController>();
      bulletScript.direction = shootDirection;
  }
}
