using UnityEngine;

public class BulletController : MonoBehaviour
{
  public Vector2 direction;
  public float speed = 20;
  public float damage = 1;

  protected new Rigidbody2D rigidbody;

  [SerializeField]
  private GameObject bulletHitPrefab;

  protected void Start()
  {
    rigidbody = gameObject.GetComponent<Rigidbody2D>();
  }

  protected void FixedUpdate()
  {
    Vector2 newPosition = (Vector2) transform.position + direction * speed * Time.fixedDeltaTime;
    rigidbody.MovePosition(newPosition);
  }

  protected void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("Wall"))
    {
      DestroySelf();
    }
    else if (other.CompareTag("Enemy"))
    {
      AbstractEnemy otherScript = other.GetComponent<AbstractEnemy>();
      otherScript.onBulletHit(gameObject);
      DestroySelf();
    }
  }
  
  private void DestroySelf()
  {
    Instantiate(bulletHitPrefab, transform.position, Util.GetRandomAngle());
    Destroy(gameObject);
  }
}
