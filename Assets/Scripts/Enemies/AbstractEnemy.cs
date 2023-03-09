using System.Collections;
using UnityEngine;

public abstract class AbstractEnemy : MonoBehaviour
{
  public float health;

  private SpriteRenderer spriteRenderer;
  private Material originalMaterial;
  private Material flashMaterial;
  private float flashDuration = 0.1f;
  private Coroutine flashRoutine;

  public void Start()
  {
    spriteRenderer = GetComponent<SpriteRenderer>();
    originalMaterial = spriteRenderer.material;
    flashMaterial = Resources.Load<Material>("Fonts & Materials/Flash Material");
  }

  public void onBulletHit(GameObject bullet)
  {
    health -= bullet.GetComponent<BulletController>().damage;
    if (health <= 0)
    {
      Destroy(gameObject);
    }
    else
    {
      FlashSprite();
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
    yield return new WaitForSeconds(flashDuration);
    spriteRenderer.material = originalMaterial;
    flashRoutine = null;
  }
}
