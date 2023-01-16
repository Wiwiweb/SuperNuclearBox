// using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

  [SerializeField]
  private float speed = 3;

  [SerializeField]
  private AbstractGun equippedGun;

  [SerializeField]
  private Vector2 movementDirection = new Vector2(0, 0);
  
  private new Rigidbody2D rigidbody;


  // Start is called before the first frame update
  void Start()
  {
    equippedGun = gameObject.AddComponent<Pistol>();
    rigidbody = gameObject.GetComponent<Rigidbody2D>();
  }

  // Update is called once per frame
  void Update()
  {
    // rigidbody.velocity = movementDirection * speed;

    Vector2 newPosition = (Vector2) transform.position + movementDirection * speed * Time.deltaTime;
    rigidbody.MovePosition(newPosition);

    if (Keyboard.current.oKey.wasPressedThisFrame)
    {
      Destroy(equippedGun);
      equippedGun = gameObject.AddComponent<Pistol>();
    } else if (Keyboard.current.pKey.wasPressedThisFrame)
    {
      Destroy(equippedGun);
      equippedGun = gameObject.AddComponent<MachineGun>();
    }
  }

  public void Move(InputAction.CallbackContext context)
  {
    movementDirection = context.ReadValue<Vector2>();
  }

  public void Fire(InputAction.CallbackContext context)
  {
    if (context.started)
    {
      equippedGun.onFirePush();
    } else if (context.canceled) 
    {
      equippedGun.onFireStop();
    }
  }
}
