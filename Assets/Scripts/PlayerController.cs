using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Util;

public class PlayerController : MonoBehaviour
{
  private const float HitstopOnDeath = 0.8f;
  private const float ScreenshakeOnDeath = 2;

  [SerializeField]
  private float speed = 1.5f; // Per second
  [SerializeField]
  public AbstractGun equippedGun;

  private Vector2 movementDirection = Vector2.zero;
  private Vector2 forcedMovement = Vector2.zero; // Per second, (i.e. already adjusted for Time.deltaTime)
  private bool godMode = false;

  private new Rigidbody2D rigidbody;
  private Animator animator;
  private GameObject gunSpriteObject;
  private GameObject gunRotationObject;
  private new Camera camera;
  private CameraController cameraController;

  void Start()
  {
    equippedGun = gameObject.AddComponent<Pistol>();
    rigidbody = gameObject.GetComponent<Rigidbody2D>();
    animator = gameObject.GetComponent<Animator>();
    gunRotationObject = gameObject.transform.Find("GunRotation").gameObject;
    gunSpriteObject = gunRotationObject.transform.Find("Gun").gameObject;
    camera = Camera.main;
    cameraController = camera.GetComponent<CameraController>();
  }

  void FixedUpdate()
  {
    Vector2 newPosition = (Vector2)transform.position + movementDirection * speed * Time.fixedDeltaTime + forcedMovement;
    (newPosition, _) = RoundToPixel(newPosition);
    rigidbody.MovePosition(newPosition);
    forcedMovement = Vector2.zero;
  }

  void Update()
  {
    Vector2 mousePosition = Mouse.current.position.ReadValue();
    mousePosition = camera.ScreenToWorldPoint(mousePosition);
    Vector2 shootDirection = mousePosition - (Vector2)transform.position;
    if (shootDirection.x > 0) // Flip gun sprite when pointing backwards
    {
      gunSpriteObject.transform.localScale = new Vector3(1, transform.localScale.x, 1);
    }
    else
    {
      gunSpriteObject.transform.localScale = new Vector3(1, -transform.localScale.x, 1);
    }
    Quaternion gunRotation = Quaternion.LookRotation(Vector3.forward, shootDirection);
    gunRotation = RoundRotation(gunRotation, 5);
    gunRotationObject.transform.rotation = gunRotation;
  }

  public void AddForcedMovement(Vector2 newMovement)
  {
    forcedMovement += newMovement * Time.deltaTime;
  }

  public void Die()
  {
    if (!godMode)
    {
      HitstopManager.instance.AddHitstop(HitstopOnDeath, DieAfterHitstop);
      GameManager.instance.dead = true;
    }
  }

  private void DieAfterHitstop()
  {
    cameraController.AddScreenshake(ScreenshakeOnDeath);
    Destroy(gameObject);
    UIController.instance.ToggleDeadTextVisible(true);
  }

  public void Move(InputAction.CallbackContext context)
  {
    movementDirection = context.ReadValue<Vector2>();
    animator.SetBool("walking", movementDirection != Vector2.zero);
    if (movementDirection.x < 0)
    {
      transform.localScale = new Vector3(-1, 1, 1);
    }
    else if (movementDirection.x > 0)
    {
      transform.localScale = new Vector3(1, 1, 1);
    }
  }

  public void Fire(InputAction.CallbackContext context)
  {
    if (context.started)
    {
      equippedGun.OnFirePush();
    }
    else if (context.canceled)
    {
      equippedGun.OnFireStop();
    }
  }

  public void Restart(InputAction.CallbackContext context)
  {
    if (context.started)
    {
      GameManager.instance.Restart();
    }
  }

  public void DebugPreviousGun(InputAction.CallbackContext context)
  {
    if (context.started)
    {
      Type gunType = GunManager.DebugGetPreviousGun();
      switchToGun(gunType, transform.position);
    }
  }

  public void DebugNextGun(InputAction.CallbackContext context)
  {
    if (context.started)
    {
      Type gunType = GunManager.DebugGetNextGun();
      switchToGun(gunType, transform.position);
    }
  }

  public void DebugGodMode(InputAction.CallbackContext context)
  {
    if (context.started)
    {
      godMode = !godMode;
      string onOff = godMode ? "on" : "off";
      string text = $"God mode {onOff}!".ToUpper();
      GameManager.instance.CreateFloatingText(transform.position, text);
    }
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
    Type newGunType = GunManager.GetRandomGunType();
    switchToGun(newGunType, box.transform.position);
    Destroy(box);
    GameManager.instance.IncrementBoxScore();
    GameManager.instance.SpawnBox();
  }

  private void switchToGun(Type gunType, Vector3 floatingTextPosition)
  {
    Destroy(equippedGun);
    equippedGun = gameObject.AddComponent(gunType) as AbstractGun;
    GameManager.instance.CreateFloatingText(floatingTextPosition, equippedGun.GunName.ToUpper() + "!");
  }
}
