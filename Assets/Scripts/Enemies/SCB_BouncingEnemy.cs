using System;
using UnityEngine;
using Random = UnityEngine.Random;
using static Util;

public class SCB_BouncingEnemy : AbstractEnemy
{
  [SerializeField]
  private float speed;

  [SerializeField]
  private Vector2 movementDirection;
  private new Rigidbody2D rigidbody;
  private Vector2 subPixelPosition = Vector2.zero; // Necessary for moving at every angle

  new void Start()
  {
    rigidbody = gameObject.GetComponent<Rigidbody2D>();
    movementDirection = GetRandomAngleVector();
    if (movementDirection.x < 0) // Flip sprite
    {
      transform.localScale = new Vector3(-1, 1, 1);
    }

    base.Start();
  }

  void FixedUpdate()
  {
    Vector2 newPosition = (Vector2)transform.position - subPixelPosition + movementDirection * speed * Time.fixedDeltaTime;
    (newPosition, subPixelPosition) = RoundToPixel(newPosition);
    rigidbody.MovePosition(newPosition);
  }


  protected override void OnCollisionEnter2D(Collision2D collision)
  {
    movementDirection = Util.CollisionBounce(collision, transform, movementDirection);
    base.OnCollisionEnter2D(collision);
  }

  protected void OnCollisionStay2D(Collision2D collision)
  {
    movementDirection = Util.CollisionBounce(collision, transform, movementDirection);
  }
}
