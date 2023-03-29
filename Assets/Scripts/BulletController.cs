using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
  public Vector2 direction;
  public float speed = 20;
  public float damage = 1;

  private new Rigidbody2D rigidbody;

  void Start()
  {
    rigidbody = gameObject.GetComponent<Rigidbody2D>();
  }

  void Update()
  {
    Vector2 newPosition = (Vector2) transform.position + direction * speed * Time.deltaTime;
    rigidbody.MovePosition(newPosition);
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("Wall"))
    {
      Destroy(gameObject);
    }
    else if (other.CompareTag("Enemy"))
    {
      AbstractEnemy otherScript = other.GetComponent<AbstractEnemy>();
      otherScript.onBulletHit(gameObject);
      Destroy(gameObject);
    }
  }
}
