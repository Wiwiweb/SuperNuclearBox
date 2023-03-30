using System.Collections;
using UnityEngine;
using static Util;

public class SCB_FlameSkull : AbstractEnemy
{
  [SerializeField]
  private float speed;
  [SerializeField]
  private float timeIdle;
  [SerializeField]
  private float timeMoving;

  private bool moving;
  private new Rigidbody2D rigidbody;
  private Vector2 subPixelPosition = Vector2.zero; // Necessary for moving at every angle

  new void Start()
  {
    moving = false;
    rigidbody = gameObject.GetComponent<Rigidbody2D>();
    if (GameManager.instance.player.transform.position.x < transform.position.x) // Make sprite look left
    {
      transform.localScale = new Vector3(-1, 1, 1);
    }
    StartCoroutine(StopMovingRoutine());

    base.Start();
  }

  void FixedUpdate()
  {
    if (moving && !GameManager.instance.dead)
    {
      Vector2 movementDirection = GameManager.instance.player.transform.position - transform.position;
      movementDirection.Normalize();
      Vector2 newPosition = (Vector2)transform.position - subPixelPosition + movementDirection * speed * Time.fixedDeltaTime;
      (newPosition, subPixelPosition) = RoundToPixel(newPosition);
      rigidbody.MovePosition(newPosition);
    }
  }

  private IEnumerator StopMovingRoutine()
  {
    moving = false;
    yield return new WaitForSeconds(timeIdle);
    StartCoroutine(StartMovingRoutine());
  }

  private IEnumerator StartMovingRoutine()
  {
    moving = true;
    yield return new WaitForSeconds(timeMoving);
    StartCoroutine(StopMovingRoutine());
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.collider.gameObject.tag.Equals("Player"))
    {
      collision.collider.gameObject.GetComponent<PlayerController>().Die();
    }
  }
}
