using UnityEngine;
using UnityEngine.InputSystem;

public abstract class AbstractGun : MonoBehaviour
{
  public abstract string gunName
  {
    get;
  }
  public abstract void onFirePush();
  public abstract void onFireStop();

  private new Camera camera;
  private GameObject gunSpriteObject;
  private float gunWidth;

  protected Sprite gunSprite;
  protected GameObject bulletPrefab;
  protected GameObject muzzleFlashPrefab;
  protected float cantFireUntil = 0;

  [SerializeField]
  protected float cooldown = 0f;
  [SerializeField]
  protected float spread = 0f;


  public void Start()
  {
    camera = Camera.main.GetComponent<Camera>();
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
    float inaccuracy = Random.Range(-spread / 2, spread / 2);
    shootDirection = Quaternion.AngleAxis(inaccuracy, Vector3.forward) * shootDirection;

    Vector2 edgeOfGun = gunSpriteObject.transform.position + gunSpriteObject.transform.right * gunWidth / 2 * transform.localScale.x;
    Quaternion lookRotation = Quaternion.LookRotation(Vector3.forward, shootDirection);
    Instantiate(muzzleFlashPrefab, edgeOfGun, lookRotation);
    GameObject bullet = Instantiate(bulletPrefab, edgeOfGun, lookRotation);
    BulletController bulletScript = bullet.GetComponent<BulletController>();
    if (bulletScript is null)
    {
      LimitedRangeBulletController s = bullet.GetComponent<LimitedRangeBulletController>();
      s.direction = shootDirection;
    }
    else
    {
      bulletScript.direction = shootDirection;
    }
  }
}
