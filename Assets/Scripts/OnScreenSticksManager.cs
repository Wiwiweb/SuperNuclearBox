using UnityEngine;
using UnityEngine.UIElements;
using System;

public class OnScreenSticksManager : MonoBehaviour
{
  public static OnScreenSticksManager instance;

  private UIToolkitOnScreenStick leftStick;
  private UIToolkitOnScreenStick rightStick;

  private GameObject player;
  private PlayerInputComponent playerInputComponent;
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
    Action<Vector2> leftAction = (stickPosition) =>
      {
        if (playerInputComponent != null)
        {
          playerInputComponent.Move(stickPosition);
        }
      };
    Action<Vector2> rightAction = (stickPosition) =>
      {
        if (playerInputComponent != null)
        {
          if (stickPosition == Vector2.zero)
          {
            playerInputComponent.StopFire();
          }
          else
          {
            playerInputComponent.Look(stickPosition);
            playerInputComponent.StartFire(); // Rapid-fire every frame
          }
        }
      };

    leftStick = new UIToolkitOnScreenStick("Left", root, leftAction);
    rightStick = new UIToolkitOnScreenStick("Right", root, rightAction);
  }

  public void SetVisibility(bool visibility)
  {
    leftStick.SetVisibility(visibility);
    rightStick.SetVisibility(visibility);
  }
}
