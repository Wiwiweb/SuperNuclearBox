using UnityEngine;

public class ExplosionController : MonoBehaviour
{
  [SerializeField]
  private float screenshake;
  [SerializeField]
  private float damage;

  protected void Start()
  {
    Camera.main.GetComponent<CameraController>().AddScreenshake(screenshake);
  }

  protected void OnTriggerEnter2D(Collider2D other)
  {
    Vector2 contactPoint = other.ClosestPoint(transform.position);
    if (other.CompareTag("Enemy"))
    {
      AbstractEnemy otherScript = other.GetComponent<AbstractEnemy>();
      otherScript.OnBulletHit(damage, contactPoint - (Vector2)transform.position);
    }
    else if (other.CompareTag("Player"))
    {
      PlayerController otherScript = other.GetComponent<PlayerController>();
      otherScript.Die(transform.position);
    }
  }
}
