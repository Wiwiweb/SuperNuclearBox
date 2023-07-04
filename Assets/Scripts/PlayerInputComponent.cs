using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInputComponent : MonoBehaviour
{
  private PlayerController playerController;
  private Animator animator;

  void Awake()
  {
    playerController = GetComponent<PlayerController>();
    animator = GetComponent<Animator>();
  }

  void Start()
  {
    UpdateOnScreenStickVisibility(GetComponent<PlayerInput>().currentControlScheme);
  }

  public void Move(InputAction.CallbackContext context)
  {
    Move(context.ReadValue<Vector2>());
  }

  public void Move(Vector2 movementDirection)
  {
    if (!playerController.Dead)
    {
      playerController.MovementDirection = movementDirection;
      animator.SetBool("walking", playerController.MovementDirection != Vector2.zero);
      if (playerController.MovementDirection.x < 0)
      {
        transform.localScale = new Vector3(-1, 1, 1);
      }
      else if (playerController.MovementDirection.x > 0)
      {
        transform.localScale = new Vector3(1, 1, 1);
      }
    }
  }

  public void Look(InputAction.CallbackContext context)
  {
    if (context.performed)
    {
      Vector2 screenLookPosition = context.ReadValue<Vector2>();
      screenLookPosition.x = Mathf.Clamp(screenLookPosition.x, 0, Camera.main.pixelWidth);
      screenLookPosition.y = Mathf.Clamp(screenLookPosition.y, 0, Camera.main.pixelHeight);
      Vector2 worldLookPosition = Camera.main.ScreenToWorldPoint(screenLookPosition);
      playerController.LookVector = worldLookPosition - (Vector2)transform.position;
    }
  }

  public void Look(Vector2 LookVector)
  {
    playerController.LookVector = LookVector;
  }

  public void Fire(InputAction.CallbackContext context)
  {
    if (context.performed)
    {
      StartFire();
    }
    else if (context.canceled)
    {
      StopFire();
    }
  }

  public void StartFire()
  {
    if (!playerController.Dead)
    {
      playerController.equippedGun.OnFirePush();
    }
  }

  public void StopFire()
  {
    playerController.equippedGun.OnFireStop();
  }

  public void Restart(InputAction.CallbackContext context)
  {
    if (context.performed)
    {
      PersistentData.BoxScore = 0;
      PersistentData.PlayerEquippedGunType = null;
      SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
  }

  public void DebugPreviousGun(InputAction.CallbackContext context)
  {
    if (context.performed)
    {
      Type gunType = GunManager.DebugGetPreviousGun();
      playerController.switchToGun(gunType, transform.position);
    }
  }

  public void DebugNextGun(InputAction.CallbackContext context)
  {
    if (context.performed)
    {
      Type gunType = GunManager.DebugGetNextGun();
      playerController.switchToGun(gunType, transform.position);
    }
  }

  public void DebugGodMode(InputAction.CallbackContext context)
  {
    if (context.performed)
    {
      playerController.GodMode = !playerController.GodMode;
      string onOff = playerController.GodMode ? "on" : "off";
      string text = $"God mode {onOff}!".ToUpper();
      GameManager.instance.CreateFloatingText(transform.position, text);
    }
  }

  public void OnControlSchemeChanged(PlayerInput playerInput)
  {
    UpdateOnScreenStickVisibility(playerInput.currentControlScheme);
  }

  private void UpdateOnScreenStickVisibility(string controlScheme)
  {
    OnScreenSticksManager.instance.SetVisibility(controlScheme == "Touch");
  }
}
