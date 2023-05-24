using UnityEngine;
using static Util;

public class BoxArrowController : MonoBehaviour
{
  [SerializeField]
  private int rotationRounding = 5;

  private new Renderer renderer;

  private void Start()
  {
    renderer = GetComponent<Renderer>();
  }

  public void UpdatePosition()
  {
    if (GameManager.instance.box == null)
    {
      renderer.enabled = false;
    }
    else
    {
      Vector2 boxPosition = GameManager.instance.box.transform.position;
      Camera camera = Camera.main;
      Vector2 boxScreenPosition = camera.WorldToViewportPoint(boxPosition);
      if (boxScreenPosition.x >= 0 && boxScreenPosition.x <= 1 && boxScreenPosition.y >= 0 && boxScreenPosition.y <= 1)
      {
        // On screen
        renderer.enabled = false;
      }
      else
      {
        renderer.enabled = true;

        // Map from 0,1 to -1,1
        boxScreenPosition = new Vector2(boxScreenPosition.x - 0.5f, boxScreenPosition.y - 0.5f) * 2;
        // Which edge is closer?
        float max = Mathf.Max(Mathf.Abs(boxScreenPosition.x), Mathf.Abs(boxScreenPosition.y));
        // Clamp to screen edge
        Vector2 arrowScreenPosition = (boxScreenPosition / max) * 0.8f;
        // Revert map, from -1,1 to 0,1
        Vector2 newArrowScreenPosition = (arrowScreenPosition / 2) + new Vector2(0.5f, 0.5f);
        Vector2 position = camera.ViewportToWorldPoint(newArrowScreenPosition);
        // position = RoundToPixel(position);
        transform.position = position;

        Vector2 direction = boxPosition - (Vector2)transform.position;
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);
        rotation = RoundRotation(rotation, rotationRounding);
        transform.rotation = rotation;
      }
    }
  }
}
