using UnityEngine;

public class LaserController : BulletController
{
  private const float HitboxRadius = 0.05f;

  private LineRenderer lineRenderer;
  private float startTime;

  protected override void Start()
  {
    startTime = Time.time;
    lineRenderer = GetComponent<LineRenderer>();

    RaycastHit2D wallHit = Physics2D.Raycast(transform.position, direction, Mathf.Infinity, LayerMask.GetMask("Wall"));
    float distanceToWall = wallHit.distance;
    lineRenderer.SetPosition(0, transform.position);
    lineRenderer.SetPosition(1, wallHit.point);

    RaycastHit2D[] enemyHits = Physics2D.CircleCastAll(transform.position, HitboxRadius, direction, distanceToWall, LayerMask.GetMask(new string []{"Enemy unit", "Flying enemy unit"}));
    foreach (RaycastHit2D enemyhit in enemyHits)
    {
      AbstractEnemy enemyScript = enemyhit.collider.gameObject.GetComponent<AbstractEnemy>();
      enemyScript.OnBulletHit(damage, direction);
    }
  }

  protected override void FixedUpdate() { }
}
