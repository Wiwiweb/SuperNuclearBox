using UnityEngine;

public class DiscController : BulletController
{
  [SerializeField]
  private int nbBounces;

  private int bouncesLeft;

  protected new void Start()
  {
    bouncesLeft = nbBounces;
    base.Start();
  }

  protected new void OnTriggerEnter2D(Collider2D other)
  {
    Vector2 contactPoint = other.ClosestPoint(transform.position);
    if (other.CompareTag("Wall"))
    {
      if (bouncesLeft > 0)
      {
        bouncesLeft--;
        // direction = Util.CollisionBounce(collision, transform, direction);
      }
      else
      {
        DestroySelf(hitWallSound, contactPoint);
      }
    }
    else if (other.CompareTag("Enemy"))
    {
      OnEnemyHit(other);
    }
  }

  protected void OnTriggerStay2D(Collider2D other)
  {
    if (other.CompareTag("Enemy"))
    {
      OnEnemyHit(other);
    }
  }

  private void OnEnemyHit(Collider2D enemy)
  {
    AbstractEnemy otherScript = enemy.GetComponent<AbstractEnemy>();
    otherScript.OnBulletHit(gameObject);
  }
}
