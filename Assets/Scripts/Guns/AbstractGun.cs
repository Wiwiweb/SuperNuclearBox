using UnityEngine;
using UnityEngine.InputSystem;

public abstract class AbstractGun : MonoBehaviour
{
  public abstract string GunName { get; }
  protected abstract string GunSpritePath { get; }
  protected abstract string BulletPrefabPath { get; }
  protected abstract string MuzzleFlashPrefabPath { get; }
  [field: SerializeField]
  protected virtual float Cooldown { get; } = 0;
  [field: SerializeField]
  protected virtual float RandomSpread { get; } = 0;
  [field: SerializeField]
  protected virtual float FixedSpread { get; } = 0;
  [field: SerializeField]
  protected virtual int NbProjectiles { get; } = 1;
  [field: SerializeField]
  protected virtual float Recoil { get; } = 0;

  public virtual void OnFirePush() { }
  public virtual void OnFireStop() { }

  private new Camera camera;
  private PlayerController playerController;
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
    playerController = gameObject.GetComponent<PlayerController>();
    gunSpriteObject = gameObject.transform.Find("GunRotation").transform.Find("Gun").gameObject;
    SpriteRenderer spriteRenderer = gunSpriteObject.GetComponent<SpriteRenderer>();
    spriteRenderer.sprite = gunSprite;
    gunWidth = spriteRenderer.bounds.size.x;
  }

  protected void createBulletsTowardsCursor()
  {
    Vector2 mousePosition = Mouse.current.position.ReadValue();
    mousePosition = camera.ScreenToWorldPoint(mousePosition);

    Vector2 lookDirection = mousePosition - (Vector2)transform.position;
    lookDirection = lookDirection.normalized;
    Vector2 edgeOfGun = gunSpriteObject.transform.position + gunSpriteObject.transform.right * gunWidth / 2 * transform.localScale.x;
    Quaternion lookRotation = Quaternion.LookRotation(Vector3.forward, lookDirection);
    Instantiate(muzzleFlashPrefab, edgeOfGun, lookRotation);

    if (Recoil > 0)
    { 
      Vector2 recoil = lookDirection * -1 * Recoil;
      playerController.AddForcedMovement(recoil);
    }

    float fixedSpreadLimit = FixedSpread / 2;
    for (int i = 0; i < NbProjectiles; i++)
    {
      float thisFixedSpread = 0;
      if (NbProjectiles > 1)
      {
        thisFixedSpread = Mathf.LerpAngle(-fixedSpreadLimit, fixedSpreadLimit, (float)i / (NbProjectiles - 1));
      }
      createOneBullet(lookDirection, edgeOfGun, thisFixedSpread);
    }
  }

  private void createOneBullet(Vector2 lookDirection, Vector2 edgeOfGun, float fixedSpread = 0)
  {
    float spread = fixedSpread + Random.Range(-RandomSpread / 2, RandomSpread / 2);
    Vector2 shootDirection = Quaternion.AngleAxis(spread, Vector3.forward) * lookDirection;
    Quaternion shootRotation = Quaternion.LookRotation(Vector3.forward, shootDirection);

    GameObject bullet = Instantiate(bulletPrefab, edgeOfGun, shootRotation);
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
