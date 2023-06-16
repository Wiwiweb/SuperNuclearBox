using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

// OnScreenStick that works with UI Toolkit instead of a canvas image
// Ideally I would use SendValueToControl just like Unity's OnScreenStick,
// but I need "isolated input" and I can't figure out the black magic they use to make that work.
// So I just hardcoded the stick to call Move()
// Eventually Unity will come up with a OnScreenStick that works with UI Toolkit.
public class UIToolkitOnScreenStick : MonoBehaviour
{
  public static UIToolkitOnScreenStick instance;

  private VisualElement onScreenStick;
  private VisualElement onScreenStickBoundary;
  private Vector2 pointerDownPosition;
  private float movementRange;
  private GameObject player;
  private PlayerInputComponent playerInputComponent;

  private InputAction pointerDownAction;
  private InputAction pointerMoveAction;

  public GameObject Player
  {
    get => player;
    set { player = value; playerInputComponent = player.GetComponent<PlayerInputComponent>(); }
  }

  void Awake()
  {
    instance = this;
  }

  void OnEnable()
  {
    VisualElement root = GetComponent<UIDocument>().rootVisualElement;
    onScreenStick = root.Q("OnScreenStick");
    onScreenStickBoundary = root.Q("OnScreenStickBoundary");

    onScreenStick.RegisterCallback<PointerDownEvent>(OnPointerDown);
    onScreenStick.RegisterCallback<PointerUpEvent>(OnPointerUp);
    onScreenStick.RegisterCallback<PointerMoveEvent>(OnPointerMove);
  }

  void OnPointerDown(PointerDownEvent eventData)
  {
    onScreenStick.CapturePointer(eventData.pointerId);
    pointerDownPosition = eventData.position;
  }

  void OnPointerUp(PointerUpEvent e)
  {
    onScreenStick.ReleasePointer(e.pointerId);
    onScreenStick.transform.position = Vector3.zero;

    if (playerInputComponent != null)
    {
      playerInputComponent.Move(Vector2.zero);
    }
    // SendValueToControl(Vector2.zero);
  }

  void OnPointerMove(PointerMoveEvent e)
  {
    if (!onScreenStick.HasPointerCapture(e.pointerId))
      return;
    Vector2 pointerCurrentPosition = (Vector2)e.position;
    Vector2 pointerDelta = pointerCurrentPosition - pointerDownPosition;
    movementRange = Mathf.Min(
      (onScreenStickBoundary.worldBound.height - onScreenStick.worldBound.height) / 2,
      (onScreenStickBoundary.worldBound.width - onScreenStick.worldBound.width) / 2
    );
    pointerDelta = Vector2.ClampMagnitude(pointerDelta, movementRange);
    onScreenStick.transform.position = pointerDelta;

    Vector2 newPos = new Vector2(pointerDelta.x / movementRange, -pointerDelta.y / movementRange); // Minus Y because UI Y axis is inverted
    if (playerInputComponent != null)
    {
      playerInputComponent.Move(newPos);
    }
    // SendValueToControl(newPos);
  }

  public void SetVisibility(bool visibility)
  {
    onScreenStick.visible = visibility;
    onScreenStickBoundary.visible = visibility;
  }
}
