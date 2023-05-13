using System;
using System.Collections;
using UnityEngine;

public abstract class AbstractEnemy : MonoBehaviour
{
  [SerializeField]
  private float maxHealth;
  [SerializeField]
  private AudioClip hitSound;
  [SerializeField]
  private AudioClip deathSound;


  private float health;

  private CameraController cameraController;
  private SpriteRenderer spriteRenderer;
  private AudioSource audioSource;
  private Material originalMaterial;
  private Material flashMaterial;
  private Coroutine flashRoutine;

  private const float FlashDuration = 0.1f;
  private const float HitStopDurationOnHit = (float)20 / 1000;
  private const float HitStopDurationOnDeath = 4 * HitStopDurationOnHit;
  private const float ScreenshakeOnHit = 0.1f;
  private const float ScreenshakeOnDeathPerMaxHealth = 0.1f;

  public void Start()
  {
    cameraController = Camera.main.GetComponent<CameraController>();
    spriteRenderer = GetComponent<SpriteRenderer>();
    audioSource = GetComponent<AudioSource>();
    originalMaterial = spriteRenderer.material;
    flashMaterial = Resources.Load<Material>("Fonts & Materials/Flash Material");
    health = maxHealth;
  }

  public void onBulletHit(GameObject bullet)
  {
    if (health > 0)
    {
      health -= bullet.GetComponent<BulletController>().damage;
      FlashSprite();
      if (health > 0)
      {
        audioSource.PlayOneShot(hitSound);
        cameraController.AddScreenshake(ScreenshakeOnHit);
        Hitstop.Add(HitStopDurationOnHit);
      }
      else
      {
        AudioSource.PlayClipAtPoint(deathSound, (Vector2) transform.position);
        cameraController.AddScreenshake(ScreenshakeOnDeathPerMaxHealth * maxHealth);
        Action callback = () => { Destroy(gameObject); };
        Hitstop.Add(HitStopDurationOnDeath, callback);
      }
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

  protected virtual void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.collider.gameObject.tag.Equals("Player"))
    {
      collision.collider.gameObject.GetComponent<PlayerController>().Die(transform.position);
    }
  }
}
