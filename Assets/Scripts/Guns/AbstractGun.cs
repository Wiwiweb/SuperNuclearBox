using UnityEngine;
using UnityEngine.InputSystem;

public abstract class AbstractGun : MonoBehaviour
{
  public abstract string GunName { get; }
  protected abstract string GunSpritePath { get; }
  protected abstract string BulletPrefabPath { get; }
  protected abstract string MuzzleFlashPrefabPath { get; }
  [field: SerializeField]
  protected virtual float Cooldown { get; } = 0f;
  [field: SerializeField]
  protected virtual float Spread { get; } = 0f;
  [field: SerializeField]
  protected virtual int NbProjectiles { get; } = 1;

  public virtual void OnFirePush() { }
  public virtual void OnFireStop() { }

  private new Camera camera;
  private GameObject gunSpriteObject;
  private float gunWidth;

  protected float cantFireUntil = 0;

  protected Sprite gunSprite;
  protected GameObject bulletPrefab;
  protected GameObject muzzleFlashPrefab;


  public void Awake()
  {
    gunSprite = Resources.Load<Sprite>(GunSpritePath);
    bulletPrefab = Resources.Load<GameObject>(BulletPrefabPath);
    muzzleFlashPrefab = Resources.Load<GameObject>(MuzzleFlashPrefabPath);

    camera = Camera.main.GetComponent<Camera>();
    gunSpriteObject = gameObject.transform.Find("GunRotation").transform.Find("Gun").gameObject;
    SpriteRenderer spriteRenderer = gunSpriteObject.GetComponent<SpriteRenderer>();
    spriteRenderer.sprite = gunSprite;
    gunWidth = spriteRenderer.bounds.size.x;
  }

  protected void createNBulletsTowardsCursor()
  {
    for (int i = 0; i < NbProjectiles; i++)
    {
      createBulletTowardsCursor();
    }
  }

  protected void createBulletTowardsCursor()
  {
    Vector2 mousePosition = Mouse.current.position.ReadValue();
    mousePosition = camera.ScreenToWorldPoint(mousePosition);

    Vector2 shootDirection = mousePosition - (Vector2)transform.position;
    shootDirection = shootDirection.normalized;
    float inaccuracy = Random.Range(-Spread / 2, Spread / 2);
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
