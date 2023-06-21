using UnityEngine;

public class GrenadeController : BulletController
{
  private const float flashTimeBeforeExplosion = 0.3f;

  [SerializeField]
  private float explodeAfterTime;
  [SerializeField]
  private Material flashMaterial;

  private float explodeTime;
  private bool flashing = false;

  protected override void Start()
  {
    base.Start();
    explodeTime = Time.time + explodeAfterTime;
    rigidbody.AddForce(direction * speed * SpeedToForce);
  }

  protected override void FixedUpdate()
  {
    float now = Time.time;
    if (explodeTime <= now)
    {
      DestroySelf(null, transform.position);
    }
    else if (!flashing && explodeTime - flashTimeBeforeExplosion <= now)
    {
      flashing = true;
      GetComponent<SpriteRenderer>().material = flashMaterial;
    }
  }

  void OnCollisionEnter2D(Collision2D collision)
  {
    ContactPoint2D[] contactPoints = new ContactPoint2D[collision.contactCount];
    collision.GetContacts(contactPoints);

    Collider2D other = collision.collider;
    if (other.CompareTag("Wall"))
    {
      AudioSource.PlayClipAtPoint(hitWallSound, transform.position);
    }
    else if (other.CompareTag("Enemy"))
    {
      Vector2 contactPoint = other.ClosestPoint(transform.position);
      AbstractEnemy otherScript = other.GetComponent<AbstractEnemy>();
      otherScript.OnBulletHit(damage, direction);
      DestroySelf(hitFleshSound, contactPoint);
    }
  }
}
