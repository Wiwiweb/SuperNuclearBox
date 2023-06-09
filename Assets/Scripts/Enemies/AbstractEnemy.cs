using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class AbstractEnemy : MonoBehaviour
{
  [SerializeField]
  private GameObject corpsePrefab;
  [SerializeField]
  private AudioClip hitSound;
  [SerializeField]
  private AudioClip deathSound;

  [SerializeField]
  private float maxHealth;

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

  protected void Start()
  {
    cameraController = Camera.main.GetComponent<CameraController>();
    spriteRenderer = GetComponent<SpriteRenderer>();
    audioSource = GetComponent<AudioSource>();
    originalMaterial = spriteRenderer.material;
    flashMaterial = Resources.Load<Material>("Fonts & Materials/Flash Material");
    health = maxHealth;
  }

  public void OnBulletHit(GameObject bullet, float hitEffectsMultiplier = 1)
  {
    if (health > 0)
    {
      BulletController bulletController = bullet.GetComponent<BulletController>();
      health -= bulletController.damage;
      FlashSprite();
      if (health > 0)
      {
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.PlayOneShot(hitSound);
        cameraController.AddScreenshake(ScreenshakeOnHit * hitEffectsMultiplier);
        Hitstop.Add(HitStopDurationOnHit * hitEffectsMultiplier);
      }
      else
      {
        AudioSource.PlayClipAtPoint(deathSound, (Vector2)transform.position);
        cameraController.AddScreenshake(ScreenshakeOnDeathPerMaxHealth * maxHealth);
        Action callback = () =>
        {
          GameObject corpse = Instantiate(corpsePrefab, transform.position, transform.rotation);
          CorpseController corpseController = corpse.GetComponent<CorpseController>();
          corpseController.Initialize(bulletController.direction, spriteRenderer.sprite, transform.localScale);
          Destroy(gameObject);
        };
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
