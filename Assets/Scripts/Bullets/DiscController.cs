using System.Collections.Generic;
using UnityEngine;

public class DiscController : BulletController
{
  private const float PlayerImmunityTime = 0.1f;

  [SerializeField]
  private int nbBounces;
  [SerializeField]
  protected float hitInvincibility;
  [SerializeField]
  protected float hitEffectsMultiplier;

  private int bouncesLeft;
  private Dictionary<GameObject, float> enemiesToIgnoreUntil = new Dictionary<GameObject, float>();
  private float dontKillPlayerUntil;

  protected new void Start()
  {
    bouncesLeft = nbBounces;
    dontKillPlayerUntil = Time.time + PlayerImmunityTime;
    base.Start();
  }

  protected new void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("Wall"))
    {
      OnWallHit(other);
    }
    else if (other.CompareTag("Enemy"))
    {
      OnEnemyHit(other);
    }
    else if (other.CompareTag("Player"))
    {
      // Friendly fire, hehe
      if (dontKillPlayerUntil < Time.time)
      {
        PlayerController otherScript = other.GetComponent<PlayerController>();
        otherScript.Die(transform.position);
      }
    }
  }

  protected void OnTriggerStay2D(Collider2D other)
  {
    if (other.CompareTag("Wall"))
    {
      OnWallHit(other);
    }
    else if (other.CompareTag("Enemy"))
    {
      OnEnemyHit(other);

    }
  }

  private void OnWallHit(Collider2D wall)
  {
    Vector2 contactPoint = wall.ClosestPoint(transform.position);
    if (bouncesLeft > 0)
    {
      bouncesLeft--;
      AudioSource.PlayClipAtPoint(hitWallSound, contactPoint);
      direction = Util.CollisionBounce(contactPoint, transform, direction);
    }
    else
    {
      DestroySelf(hitWallSound, contactPoint);
    }
  }

  private void OnEnemyHit(Collider2D enemy)
  {
    // Don't hit enemies we already hit recently
    float ignoreUntil;
    bool hasKey = enemiesToIgnoreUntil.TryGetValue(enemy.gameObject, out ignoreUntil);
    if (!hasKey || ignoreUntil < Time.time)
    {
      AbstractEnemy otherScript = enemy.GetComponent<AbstractEnemy>();
      otherScript.OnBulletHit(damage, direction, hitEffectsMultiplier);
      enemiesToIgnoreUntil[enemy.gameObject] = Time.time + hitInvincibility;
    }
  }
}
