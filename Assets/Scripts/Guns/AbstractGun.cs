using UnityEngine;
using UnityEngine.InputSystem;

public abstract class AbstractGun : MonoBehaviour
{
  public abstract void onFirePush();
  public abstract void onFireStop();

  private new Camera camera;
  private GameObject gunSpriteObject;
  private float gunWidth;

  protected Sprite gunSprite;
  protected GameObject bulletPrefab;
  protected float cooldown = 0f;
  protected float spread = 0f;
  protected float cantFireUntil = 0;

  public void Start()
  {
    camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    gunSpriteObject = gameObject.transform.Find("GunRotation").transform.Find("Gun").gameObject;
    SpriteRenderer spriteRenderer = gunSpriteObject.GetComponent<SpriteRenderer>();
    spriteRenderer.sprite = gunSprite;
    gunWidth = spriteRenderer.bounds.size.x;
  }

  protected void createBulletTowardsCursor(GameObject bulletPrefab)
  {
      Vector2 mousePosition = Mouse.current.position.ReadValue();
      mousePosition = camera.ScreenToWorldPoint(mousePosition);

      Vector2 shootDirection = mousePosition - (Vector2)transform.position;
      shootDirection = shootDirection.normalized;
      float inaccuracy = Random.Range(-spread/2, spread/2);
      shootDirection = Quaternion.AngleAxis(inaccuracy, Vector3.forward) * shootDirection;

      Vector2 edgeOfGun = gunSpriteObject.transform.position + gunSpriteObject.transform.right * gunWidth/2 * transform.localScale.x;
      GameObject bullet = Instantiate(bulletPrefab, edgeOfGun, Quaternion.LookRotation(Vector3.forward, shootDirection));
      BulletController bulletScript = bullet.GetComponent<BulletController>();
      bulletScript.direction = shootDirection;
  }
}
