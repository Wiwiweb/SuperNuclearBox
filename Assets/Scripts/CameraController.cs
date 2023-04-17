using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
  private const float Height = 3;

  public GameObject player;

  [SerializeField]
  private BoxArrowController boxArrowController;

  private Vector2 kickback = Vector2.zero;

  void Update()
  {
    if (!GameManager.instance.dead)
    {
      Vector2 mousePosition = Mouse.current.position.ReadValue();
      mousePosition.x = Mathf.Clamp(mousePosition.x, 0, Camera.main.pixelWidth);
      mousePosition.y = Mathf.Clamp(mousePosition.y, 0, Camera.main.pixelHeight);
      mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
      Vector2 cameraPosition = (mousePosition + (Vector2)player.transform.position * 3) / 4;
      cameraPosition += kickback;
      kickback = Vector2.zero;

      transform.position = new Vector3(cameraPosition.x, cameraPosition.y, -Height);
    }
  }

  void OnPreRender()
  {
    boxArrowController.UpdatePosition();
  }

  public void AddKickback(Vector2 addedKickback)
  {
    kickback += addedKickback * Time.deltaTime;
  }
}
