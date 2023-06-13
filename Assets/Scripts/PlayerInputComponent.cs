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

  public void Move(InputAction.CallbackContext context)
  {
    if (!playerController.Dead)
    {
      playerController.MovementDirection = context.ReadValue<Vector2>();
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

  public void Fire(InputAction.CallbackContext context)
  {
     if (!playerController.Dead)
    {
      if (context.started)
      {
        playerController.equippedGun.OnFirePush();
      }
      else if (context.canceled)
      {
        playerController.equippedGun.OnFireStop();
      }
    }
  }

  public void Restart(InputAction.CallbackContext context)
  {
    if (context.started)
    {
      PersistentData.BoxScore = 0;
      PersistentData.PlayerEquippedGunType = null;
      SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
  }

  public void DebugPreviousGun(InputAction.CallbackContext context)
  {
    if (context.started)
    {
      Type gunType = GunManager.DebugGetPreviousGun();
      playerController.switchToGun(gunType, transform.position);
    }
  }

  public void DebugNextGun(InputAction.CallbackContext context)
  {
    if (context.started)
    {
      Type gunType = GunManager.DebugGetNextGun();
      playerController.switchToGun(gunType, transform.position);
    }
  }

  public void DebugGodMode(InputAction.CallbackContext context)
  {
    if (context.started)
    {
      playerController.GodMode = !playerController.GodMode;
      string onOff = playerController.GodMode ? "on" : "off";
      string text = $"God mode {onOff}!".ToUpper();
      GameManager.instance.CreateFloatingText(transform.position, text);
    }
  }
}
