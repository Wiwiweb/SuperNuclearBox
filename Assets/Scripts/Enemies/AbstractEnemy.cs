using System;
using System.Collections;
using UnityEngine;
using static GameManager;

public abstract class AbstractEnemy : MonoBehaviour
{
  public float health;

  private SpriteRenderer spriteRenderer;
  private Material originalMaterial;
  private Material flashMaterial;
  private Coroutine flashRoutine;

  private const float FlashDuration = 0.1f;
  private const float HitStopOnHitDuration = (float)20 / 1000;
  private const float HitStopOnDeathDuration = 4 * HitStopOnHitDuration;

  public void Start()
  {
    spriteRenderer = GetComponent<SpriteRenderer>();
    originalMaterial = spriteRenderer.material;
    flashMaterial = Resources.Load<Material>("Fonts & Materials/Flash Material");
  }

  public void onBulletHit(GameObject bullet)
  {
    health -= bullet.GetComponent<BulletController>().damage;
    FlashSprite();
    if (health <= 0)
    {
      Action callback = () => { Destroy(gameObject); };
      GameManager.instance.HitStop(HitStopOnDeathDuration, callback);
    }
    else
    {
      GameManager.instance.HitStop(HitStopOnHitDuration);
    }
  }

  private void FlashSprite()
  {
    if (flashRoutine != null)
    {
      StopCoroutine(flashRoutine);
    }

    flashRoutine = StartCoroutine(FlashRoutine());
  }

  private IEnumerator FlashRoutine()
  {
    spriteRenderer.material = flashMaterial;
    yield return new WaitForSeconds(FlashDuration);
    spriteRenderer.material = originalMaterial;
    flashRoutine = null;
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.collider.gameObject.tag.Equals("Player"))
    {
      collision.collider.gameObject.GetComponent<PlayerController>().Die();
    }
  }
}
