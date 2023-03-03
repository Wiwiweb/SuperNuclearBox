using UnityEngine;

public class CameraController : MonoBehaviour
{

  public GameObject player;

  [SerializeField]
  private BoxArrowController boxArrowController;

  private float height = 3;

  void Update()
  {
    UpdatePosition(player.transform.position);
  }

  public void UpdatePosition(Vector2 playerPosition)
  {
    transform.position = new Vector3(playerPosition.x, playerPosition.y, -height);
  }

  void OnPreRender()
  {
    boxArrowController.UpdatePosition();
  }
}
