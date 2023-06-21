using System;
using UnityEngine;

internal static class UtilCollision
{
  private static Vector2 AverageContactPoint(Collision2D collision)
  {
    ContactPoint2D[] contactPoints = new ContactPoint2D[collision.contactCount];
    collision.GetContacts(contactPoints);

    Vector2 averageContactPoint = Vector2.zero;
    foreach (ContactPoint2D contactPoint in contactPoints)
    {
      // Debug.DrawRay(contactPoint.point, contactPoint.normal * 0.5f, Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f), 10f);
      averageContactPoint += contactPoint.point;
    }
    averageContactPoint /= contactPoints.Length;
    return averageContactPoint;
  }

  // Find the obstacle direction based on the relative positions of the current transform and the contact point.
  // This doesn't work for high-speed objects because the transform's position is from a frame after the collision.
  private static Vector2 ObstacleDirectionFromContactPointAndTransform(Vector2 contactPoint, Transform transform)
  {
    Vector2 localContactPoint = contactPoint - (Vector2)transform.position;
    return ObstacleDirectionFromLocalContactPoint(localContactPoint);
  }

  private static Vector2 ObstacleDirectionFromLocalContactPoint(Vector2 localContactPoint)
  {
    Vector2 obstacleDirection = Vector2.zero;
    if (Math.Abs(localContactPoint.x) <= Math.Abs(localContactPoint.y)) // Horizontal wall
    {
      if (localContactPoint.y > 0) // Top hit
        obstacleDirection.y = 1;
      else // Bottom hit
        obstacleDirection.y = -1;
    }
    if (Math.Abs(localContactPoint.x) >= Math.Abs(localContactPoint.y)) // Vertical wall (not in an else, to allow for corner hits when x == y)
    {
      if (localContactPoint.x > 0) // Right hit
        obstacleDirection.x = 1;
      else // Left hit
        obstacleDirection.x = -1;
    }
    return obstacleDirection;
  }

  private static Vector2 BounceMovementDirection(Transform transform, Vector2 movementDirection, Vector2 obstacleDirection)
  {
    if (obstacleDirection.x > 0) // Right hit
    {
      // Debug.Log("Right hit");
      movementDirection.x = -Math.Abs(movementDirection.x);
      transform.localScale = new Vector3(-1, 1, 1);
    }
    else if (obstacleDirection.x < 0) // Left hit
    {
      // Debug.Log("Left hit");
      movementDirection.x = Math.Abs(movementDirection.x);
      transform.localScale = new Vector3(1, 1, 1);
    }

    if (obstacleDirection.y > 0) // Top hit
    {
      // Debug.Log("Top hit");
      movementDirection.y = -Math.Abs(movementDirection.y);
    }
    else if (obstacleDirection.y < 0) // Bottom hit
    {
      // Debug.Log("Bottom hit");
      movementDirection.y = Math.Abs(movementDirection.y);
    }

    return movementDirection;
  }

  // Providing collision, for kinematic rigidbodies
  // Doesn't work for fast objects because position is 1 frame late
  public static Vector2 CollisionBounce(Collision2D collision, Transform transform, Vector2 movementDirection)
  {
    Vector2 averageContactPoint = AverageContactPoint(collision);
    return CollisionBounce(averageContactPoint, transform, movementDirection);
  }

  // Providing contactPoint, for trigger colliders
  public static Vector2 CollisionBounce(Vector2 contactPoint, Transform transform, Vector2 movementDirection)
  {
    Vector2 obstacleDirection = ObstacleDirectionFromContactPointAndTransform(contactPoint, transform);
    return BounceMovementDirection(transform, movementDirection, obstacleDirection);
  }
}
