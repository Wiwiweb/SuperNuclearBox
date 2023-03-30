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
    float movementAngle = Random.Range(0f, 360f);
    movementDirection = new Vector2((float)Math.Cos(movementAngle), (float)Math.Sin(movementAngle));
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


  private void OnCollisionEnter2D(Collision2D collision)
  {
    movementDirection = Util.CollisionBounce(collision, transform, movementDirection);
    if (collision.collider.gameObject.tag.Equals("Player"))
    {
      collision.collider.gameObject.GetComponent<PlayerController>().Die();
    }
  }

  private void OnCollisionStay2D(Collision2D collision)
  {
    movementDirection = Util.CollisionBounce(collision, transform, movementDirection);
  }
}
