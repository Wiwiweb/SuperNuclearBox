using UnityEngine;

public class GrenadeController : BulletController
{
  private const float flashTimeBeforeExplosion = 0.1f;

  [SerializeField]
  private float speedDecrease;
  [SerializeField]
  private float explodeAfterTime;
  [SerializeField]
  private Material flashMaterial;

  private float explodeTime;
  private bool flashing = false;


  protected override void Start()
  {
    explodeTime = Time.time + explodeAfterTime;
    base.Start();
  }

  new void FixedUpdate()
  {
    if (speed > 0)
    {
      speed /= speedDecrease;
      speed = Mathf.Max(0, speed);
    }

    if (explodeTime <= Time.time)
    {
      DestroySelf(null, transform.position);
    }
    else if (!flashing && explodeTime <= Time.time - flashTimeBeforeExplosion)
    {
      GetComponent<SpriteRenderer>().material = flashMaterial;
    }
    else
    {
      base.FixedUpdate();
    }
  }

  new void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("Wall"))
    {
      OnWallHit(other);
    }
    else if (other.CompareTag("Enemy"))
    {
      Vector2 contactPoint = other.ClosestPoint(transform.position);
      AbstractEnemy otherScript = other.GetComponent<AbstractEnemy>();
      otherScript.OnBulletHit(damage, direction);
      DestroySelf(hitFleshSound, contactPoint);
    }
  }

  void OnTriggerStay2D(Collider2D other)
  {
    if (other.CompareTag("Wall"))
    {
      OnWallHit(other);
    }
  }

  private void OnWallHit(Collider2D wall)
  {
    Vector2 contactPoint = wall.ClosestPoint(transform.position);
    AudioSource.PlayClipAtPoint(hitWallSound, contactPoint);
    direction = Util.CollisionBounce(contactPoint, transform, direction);
  }
}
