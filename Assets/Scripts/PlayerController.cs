using UnityEngine;
using UnityEngine.InputSystem;
using static Util;

public class PlayerController : MonoBehaviour
{

  [SerializeField]
  private float speed = 3;

  [SerializeField]
  private AbstractGun equippedGun;

  [SerializeField]
  private Vector2 movementDirection = new Vector2(0, 0);

  private new Rigidbody2D rigidbody;
  private Animator animator;
  private GameObject gunSpriteObject;
  private GameObject gunRotationObject;
  private GameManager gameManager;
  private new Camera camera;

  void Start()
  {
    equippedGun = gameObject.AddComponent<Pistol>();
    rigidbody = gameObject.GetComponent<Rigidbody2D>();
    animator = gameObject.GetComponent<Animator>();
    gunRotationObject = gameObject.transform.Find("GunRotation").gameObject;
    gunSpriteObject = gunRotationObject.transform.Find("Gun").gameObject;
    camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
  }

  void Update()
  {
    Vector2 newPosition = (Vector2)transform.position + movementDirection * speed * Time.deltaTime;
    newPosition = RoundToPixel(newPosition);
    rigidbody.MovePosition(newPosition);

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

    if (Keyboard.current.oKey.wasPressedThisFrame)
    {
      Destroy(equippedGun);
      equippedGun = gameObject.AddComponent<Pistol>();
    }
    else if (Keyboard.current.pKey.wasPressedThisFrame)
    {
      Destroy(equippedGun);
      equippedGun = gameObject.AddComponent<MachineGun>();
    }
  }

  public void Move(InputAction.CallbackContext context)
  {
    movementDirection = context.ReadValue<Vector2>();
    animator.SetBool("walking", movementDirection != new Vector2(0, 0));
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
      equippedGun.onFirePush();
    }
    else if (context.canceled)
    {
      equippedGun.onFireStop();
    }
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.gameObject.tag.Equals("Box"))
    {
      Destroy(other);
      gameManager.spawnBox();
      
    }
  }
}
