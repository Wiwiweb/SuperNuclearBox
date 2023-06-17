using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static Util;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
  private const float HitstopOnDeath = 0.7f;
  private const float ScreenshakeOnDeath = 2;
  private const float DefaultDeathPushIntensity = 50;

  [SerializeField]
  private float speed = 1.5f; // Per second
  [SerializeField]
  public AbstractGun equippedGun;

  [SerializeField]
  private AudioClip hitSound;
  [SerializeField]
  private AudioClip deathSound;
  [SerializeField]
  private AudioClip boxPickupGoodSound;
  [SerializeField]
  private AudioClip boxPickupNormalSound;
  [SerializeField]
  private AudioClip boxPickupBadSound;

  [SerializeField]
  private GameObject portalPrefab;

  public bool Dead { get; set; }
  public bool GodMode { get; set; }
  public Vector2 MovementDirection { get; set; }
  public Vector2 LookVector { get; set; }

  private Vector2 forcedMovement = Vector2.zero; // Per second, (i.e. already adjusted for Time.deltaTime)

  private new Rigidbody2D rigidbody;
  private Animator animator;
  private AudioSource audioSource;
  private GameObject gunSpriteObject;
  private GameObject gunRotationObject;
  private new Camera camera;
  private CameraController cameraController;


  void Start()
  {
    if (PersistentData.PlayerEquippedGunType == null)
    {
      PersistentData.PlayerEquippedGunType = typeof(Pistol);
    }

    equippedGun = gameObject.AddComponent(PersistentData.PlayerEquippedGunType) as AbstractGun;
    rigidbody = GetComponent<Rigidbody2D>();
    animator = GetComponent<Animator>();
    audioSource = GetComponent<AudioSource>();
    gunRotationObject = transform.Find("GunRotation").gameObject;
    gunSpriteObject = gunRotationObject.transform.Find("Gun").gameObject;
    camera = Camera.main;
    cameraController = camera.GetComponent<CameraController>();

    audioSource.ignoreListenerPause = true;

    // TouchSimulation.Enable();
    // GetComponent<PlayerInput>().SwitchCurrentControlScheme(InputSystem.devices.First(d => d == Touchscreen.current));
  }

  void FixedUpdate()
  {
    if (!Dead)
    {
      Vector2 newPosition = (Vector2)transform.position + MovementDirection * speed * Time.fixedDeltaTime + forcedMovement;
      (newPosition, _) = RoundToPixel(newPosition);
      rigidbody.MovePosition(newPosition);
      forcedMovement = Vector2.zero;
    }
  }

  void Update()
  {
    if (!Dead)
    {
      if (LookVector.x > 0) // Flip gun sprite when pointing backwards
      {
        gunSpriteObject.transform.localScale = new Vector3(1, transform.localScale.x, 1);
      }
      else
      {
        gunSpriteObject.transform.localScale = new Vector3(1, -transform.localScale.x, 1);
      }
      Quaternion gunRotation = Quaternion.LookRotation(Vector3.forward, LookVector);
      gunRotation = RoundRotation(gunRotation, 5);
      gunRotationObject.transform.rotation = gunRotation;
    }
  }

  public void AddForcedMovement(Vector2 newMovement)
  {
    forcedMovement += newMovement * Time.deltaTime;
  }

  public void Die(Vector2 causePosition, float intensityMultiplier = 1)
  {
    if (!GodMode && !Dead)
    {
      Hitstop.Add(HitstopOnDeath, DieAfterHitstop);
      Dead = true;

      audioSource.pitch = Random.Range(0.9f, 1.3f);
      audioSource.PlayOneShot(hitSound);

      MusicController.instance.Pause();

      Vector2 causeDirection = ((Vector2)transform.position - causePosition).normalized;
      Vector2 push = causeDirection * DefaultDeathPushIntensity * intensityMultiplier;
      rigidbody.AddForce(push);

      gameObject.layer = LayerMask.NameToLayer("Corpse");
      MovementDirection = Vector2.zero;
      equippedGun.OnFireStop();
      cameraController.FixPosition();
      gunSpriteObject.GetComponent<SpriteRenderer>().enabled = false;
      animator.SetTrigger("dead");
    }
  }

  private void DieAfterHitstop()
  {
    if (gameObject != null) // Could happen if we restart before the callback
    {
      GetComponent<PlayerInput>().SwitchCurrentActionMap("Dead"); // Press any button to reset

      audioSource.pitch = Random.Range(0.9f, 1.2f);
      audioSource.PlayOneShot(deathSound);

      MusicController.instance.UnPause();

      cameraController.AddScreenshake(ScreenshakeOnDeath);
      UIController.instance.ToggleDeadTextVisible(true);
    }
  }

  public void SetAffectedBySpriteMask()
  {
    GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
    gunSpriteObject.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.gameObject.tag.Equals("Box"))
    {
      OnBoxPickup(other.gameObject);
    }
  }

  private void OnBoxPickup(GameObject box)
  {
    Vector2 boxPosition = box.transform.position;
    Type newGunType = GunManager.GetRandomGunType();
    switchToGun(newGunType, boxPosition);
    Destroy(box);
    GameManager.instance.IncrementBoxScore();
    if (PersistentData.BoxScore % GameManager.BoxesBeforeLevelSwitch == 0)
    {
      GameObject portal = Instantiate(portalPrefab, boxPosition, Quaternion.identity);
      GetComponent<SpriteRenderer>().sortingLayerName = "Flying units"; // Stay over portal
    }
    else
    {
      GameManager.instance.SpawnBox();
    }
  }

  public void switchToGun(Type gunType, Vector3 floatingTextPosition)
  {
    Destroy(equippedGun);
    equippedGun = gameObject.AddComponent(gunType) as AbstractGun;
    PersistentData.PlayerEquippedGunType = gunType;

    AudioClip pickupSound;
    switch (equippedGun.GunRarity)
    {
      case AbstractGun.GunRarityType.Good:
        pickupSound = boxPickupGoodSound;
        break;
      case AbstractGun.GunRarityType.Bad:
        pickupSound = boxPickupBadSound;
        break;
      case AbstractGun.GunRarityType.Normal:
      default:
        pickupSound = boxPickupNormalSound;
        break;
    }
    audioSource.PlayOneShot(pickupSound);

    GameManager.instance.CreateFloatingText(floatingTextPosition, equippedGun.GunName.ToUpper() + "!");
  }
}
