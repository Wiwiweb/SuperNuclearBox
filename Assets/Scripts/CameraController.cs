using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{

  public GameObject player;

  [SerializeField]
  private BoxArrowController boxArrowController;

  private float height = 3;

  void Update()
  {
    Vector2 mousePosition = Mouse.current.position.ReadValue();
    mousePosition.x = Mathf.Clamp(mousePosition.x, 0, Camera.main.pixelWidth);
    mousePosition.y = Mathf.Clamp(mousePosition.y, 0, Camera.main.pixelHeight);
    mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
    Vector2 cameraPosition = (mousePosition + (Vector2)player.transform.position * 3) / 4;
    transform.position = new Vector3(cameraPosition.x, cameraPosition.y, -height);
  }

  void OnPreRender()
  {
    boxArrowController.UpdatePosition();
  }
}
