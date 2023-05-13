using UnityEngine;

public class CorpseController : MonoBehaviour
{
  private const float forceMin = 20;
  private const float forceMax = 35;
  private const float torqueMin = -15;
  private const float torqueMax = 15;

  private Vector2 deathDirection;

  public void Initialize(Vector2 deathDirection, Sprite sprite, Vector3 scale)
  {
    gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    transform.localScale = scale;
    deathDirection.Normalize();
    this.deathDirection = deathDirection;
  }

  protected void Start()
  {
    Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D>();
    rigidbody.AddForce(Vector2.up * Random.Range(forceMin, forceMax), ForceMode2D.Impulse);
    rigidbody.AddForce(deathDirection * Random.Range(forceMin, forceMax), ForceMode2D.Impulse);
    rigidbody.AddTorque(Random.Range(torqueMin, torqueMax), ForceMode2D.Impulse);
  }

  protected void OnBecameInvisible()
  {
    Destroy(gameObject);
  }
}
