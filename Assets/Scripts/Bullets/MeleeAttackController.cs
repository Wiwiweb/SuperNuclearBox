using UnityEngine;

public class MeleeAttackController : MonoBehaviour
{
  [SerializeField]
  private float damage;

  protected void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("Enemy"))
    {
      Vector2 direction = transform.rotation * Vector3.forward;
      AbstractEnemy otherScript = other.GetComponent<AbstractEnemy>();
      otherScript.OnBulletHit(damage, direction);
    }
  }
}
