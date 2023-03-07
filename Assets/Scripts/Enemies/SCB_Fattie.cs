using System;
using UnityEngine;
using Random = UnityEngine.Random;
using static Util;

public class SCB_Fatty : AbstractEnemy
{
  [SerializeField]
  private float speed;

  [SerializeField]
  private GameObject positionMarkerPrefab;

  [SerializeField]
  private Vector2 movementDirection;
  private new Rigidbody2D rigidbody;

  void Start()
  {
    rigidbody = gameObject.GetComponent<Rigidbody2D>();

    health = 10;
    speed = 1;
    float movementAngle = Random.Range(0f, 360f);
    movementDirection = new Vector2((float)Math.Cos(movementAngle), (float)Math.Sin(movementAngle));
    if (movementDirection.x < 0) // Flip sprite
    {
      transform.localScale = new Vector3(-1, 1, 1);
    }
  }

  void FixedUpdate()
  {
    Vector2 newPosition = (Vector2)transform.position + movementDirection * speed * Time.fixedDeltaTime;
    newPosition = RoundToPixel(newPosition);
    rigidbody.MovePosition(newPosition);
  }


  private void OnCollisionEnter2D(Collision2D collision)
  {
    WallBounce(collision);
  }

  private void OnCollisionStay2D(Collision2D collision)
  {
    WallBounce(collision);
  }

  private void WallBounce(Collision2D collision)
  {
    if (collision.collider.CompareTag("Wall"))
    {
      ContactPoint2D[] contactPoints = new ContactPoint2D[collision.contactCount];
      collision.GetContacts(contactPoints);

      foreach (ContactPoint2D contactPoint in contactPoints)
      {
        Vector2 localContactPoint = transform.InverseTransformPoint(contactPoint.point);
        localContactPoint *= transform.localScale;
        if (Math.Abs(localContactPoint.x) <= Math.Abs(localContactPoint.y)) // Horizontal wall
        {
          if (localContactPoint.y > 0) // Top hit
          {
            movementDirection.y = -Math.Abs(movementDirection.y);
          }
          else // Bottom hit
          {
            movementDirection.y = Math.Abs(movementDirection.y);
          }
        }
        if (Math.Abs(localContactPoint.x) >= Math.Abs(localContactPoint.y)) // Vertical wall (not in an else, to allow for corner hits when x == y)
        {
          if (localContactPoint.x > 0) // Right hit
          {
            movementDirection.x = -Math.Abs(movementDirection.x);
            transform.localScale = new Vector3(-1, 1, 1);
          }
          else // Left hit
          {
            movementDirection.x = Math.Abs(movementDirection.x);
            transform.localScale = new Vector3(1, 1, 1);
          }
        }
      }
    }
  }
}
