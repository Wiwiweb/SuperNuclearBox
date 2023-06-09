using UnityEngine;

public class BulletController : MonoBehaviour
{
  public Vector2 direction;
  public float speed;
  public float damage;

  protected new Rigidbody2D rigidbody;

  [SerializeField]
  private GameObject bulletHitPrefab;
  [SerializeField]
  protected AudioClip hitWallSound;
  [SerializeField]
  protected AudioClip hitFleshSound;
  
  protected virtual float ScreenshakeMultiplier { get; set; } = 1;
  protected virtual float HitstopMultiplier { get; set; } = 1;

  protected void Start()
  {
    rigidbody = gameObject.GetComponent<Rigidbody2D>();
  }

  protected void FixedUpdate()
  {
    Vector2 newPosition = (Vector2)transform.position + direction * speed * Time.fixedDeltaTime;
    rigidbody.MovePosition(newPosition);
  }

  protected void OnTriggerEnter2D(Collider2D other)
  {
    Vector2 contactPoint = other.ClosestPoint(transform.position);
    if (other.CompareTag("Wall"))
    {
      DestroySelf(hitWallSound, contactPoint);
    }
    else if (other.CompareTag("Enemy"))
    {
      AbstractEnemy otherScript = other.GetComponent<AbstractEnemy>();
      otherScript.OnBulletHit(gameObject);
      DestroySelf(hitFleshSound, contactPoint);
    }
  }

  protected void DestroySelf(AudioClip hitSound, Vector2 contactPoint)
  {
    AudioSource.PlayClipAtPoint(hitSound, contactPoint);
    if (bulletHitPrefab != null)
    {
      Instantiate(bulletHitPrefab, contactPoint, Util.GetRandomAngle());
    }
    Destroy(gameObject);
  }
}
