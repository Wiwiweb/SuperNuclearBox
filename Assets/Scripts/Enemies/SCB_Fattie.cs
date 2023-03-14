using System;
using UnityEngine;
using Random = UnityEngine.Random;
using static Util;

public class SCB_Fattie : AbstractEnemy
{
  [SerializeField]
  private float speed;

  [SerializeField]
  private GameObject positionMarkerPrefab;

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
    Bounce(collision);
  }

  private void OnCollisionStay2D(Collision2D collision)
  {
    Bounce(collision);
  }

  private void Bounce(Collision2D collision)
  {
    ContactPoint2D[] contactPoints = new ContactPoint2D[collision.contactCount];
    collision.GetContacts(contactPoints);

    Vector2 averageContactPoint = Vector2.zero;
    foreach (ContactPoint2D contactPoint in contactPoints)
    {
      // Instantiate(positionMarkerPrefab, contactPoint.point, Quaternion.identity);
      averageContactPoint += contactPoint.point;
    }
    averageContactPoint /= contactPoints.Length;

    Vector2 localContactPoint = transform.InverseTransformPoint(averageContactPoint);
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
