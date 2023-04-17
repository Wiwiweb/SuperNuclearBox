using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
  private const float Height = 3;

  public GameObject player;

  [SerializeField]
  private BoxArrowController boxArrowController;

  [SerializeField]
  private float screenshakeReductionPerSec = 1;
  [SerializeField]
  private float screenshakeMultiplier = 1;

  [SerializeField]
  private Vector2 kickback = Vector2.zero;
  [SerializeField]
  private float screenshakePower = 0;

  void Update()
  {
    Vector2 targetPosition = transform.position;

    if (!GameManager.instance.dead)
    {
      Vector2 mousePosition = Mouse.current.position.ReadValue();
      mousePosition.x = Mathf.Clamp(mousePosition.x, 0, Camera.main.pixelWidth);
      mousePosition.y = Mathf.Clamp(mousePosition.y, 0, Camera.main.pixelHeight);
      mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
      targetPosition = (mousePosition + (Vector2)player.transform.position * 3) / 4;

      targetPosition += kickback;
      kickback = Vector2.zero;
    }
    
    if (screenshakePower > 0)
    {
      targetPosition += getScreenshakeVector();
      screenshakePower -= screenshakeReductionPerSec * Time.deltaTime;
      screenshakePower = Mathf.Max(0, screenshakePower);
    }

    transform.position = new Vector3(targetPosition.x, targetPosition.y, -Height);
  }

  void OnPreRender()
  {
    boxArrowController.UpdatePosition();
  }

  public void AddKickback(Vector2 addedKickback)
  {
    kickback += addedKickback * Time.deltaTime;
  }

  public void AddScreenshake(float addedScreenshake)
  {
    screenshakePower += addedScreenshake;
  }

  private Vector2 getScreenshakeVector()
  {
    Vector2 shakeDirection = Util.GetRandomAngleVector();
    return shakeDirection * screenshakePower * screenshakeMultiplier;
  }
}
