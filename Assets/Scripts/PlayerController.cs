using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static Util;

public class PlayerController : MonoBehaviour
{
  [SerializeField]
  private GameObject floatingTextPrefab;

  [SerializeField]
  private float speed = 1.5f;
  [SerializeField]
  public AbstractGun equippedGun;

  private Vector2 movementDirection = new Vector2(0, 0);

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
    Vector2 newPosition = (Vector2)transform.position + movementDirection * speed * Time.fixedDeltaTime;
    (newPosition, _) = RoundToPixel(newPosition);
    rigidbody.MovePosition(newPosition);
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

  public void Die()
  {
    Destroy(gameObject);
    GameManager.instance.dead = true;
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
    GameObject floatingText = Instantiate(floatingTextPrefab, floatingTextPosition, Quaternion.identity);
    floatingText.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(equippedGun.GunName.ToUpper() + "!");
  }
}
