using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class AbstractGun : MonoBehaviour
{
  public abstract void onFirePush();
  public abstract void onFireStop();

  private new Camera camera;
  private GameObject gunSpriteObject;

  private float gunWidth;

  public void Start()
  {
    camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    gunSpriteObject = gameObject.transform.Find("GunRotation").transform.Find("Gun").gameObject;
    gunWidth = gunSpriteObject.GetComponent<SpriteRenderer>().bounds.size.x;
  }

  protected void createBulletTowardsCursor(GameObject bulletPrefab)
  {
      Vector2 mousePosition = Mouse.current.position.ReadValue();
      mousePosition = camera.ScreenToWorldPoint(mousePosition);

      Vector2 shootDirection = mousePosition - (Vector2)transform.position;
      shootDirection = shootDirection.normalized;
      Vector2 edgeOfGun = gunSpriteObject.transform.position + gunSpriteObject.transform.right * gunWidth/2 * transform.localScale.x;
      GameObject bullet = Instantiate(bulletPrefab, edgeOfGun, Quaternion.LookRotation(Vector3.forward, shootDirection));
      BulletController bulletScript = bullet.GetComponent<BulletController>();
      bulletScript.direction = shootDirection;
  }
}
