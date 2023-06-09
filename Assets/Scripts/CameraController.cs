using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
  private const float Height = 3;

  private GameObject player;
  private PlayerController playerController;

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
  
  private bool fixPosition = false;
  private Vector2 fixedPosition;

  public GameObject Player
  {
    get => player;
    set { player = value; playerController = player.GetComponent<PlayerController>(); }
  }

  void Update()
  {
    Vector2 targetPosition;
    if (fixPosition)
    {
      targetPosition = fixedPosition;
    }
    else {
      Vector2 mousePosition = Mouse.current.position.ReadValue();
      if (mousePosition == Vector2.zero)
      {
        return;
      }
      mousePosition.x = Mathf.Clamp(mousePosition.x, 0, Camera.main.pixelWidth);
      mousePosition.y = Mathf.Clamp(mousePosition.y, 0, Camera.main.pixelHeight);
      mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
      targetPosition = (mousePosition + (Vector2)Player.transform.position * 3) / 4;

      targetPosition += kickback;
      kickback = Vector2.zero;
    }

    if (screenshakePower > 0)
    {
      targetPosition += getScreenshakeVector();
      screenshakePower *= 0.99f; // Percent decrease to make bigger shakes proportionally shorter
      screenshakePower -= screenshakeReductionPerSec * Time.deltaTime; // Flat decrease
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

  public void SetPosition(Vector2 position)
  {
    transform.position = position;
  }
  
  public void FixPosition()
  {
    fixPosition = true;
    fixedPosition = transform.position;
  }

  private Vector2 getScreenshakeVector()
  {
    Vector2 shakeDirection = Util.GetRandomAngleVector();
    return shakeDirection * screenshakePower * screenshakeMultiplier;
  }
}
