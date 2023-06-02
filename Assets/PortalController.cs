using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalController : MonoBehaviour
{
  private const float SecondsBeforeClose = 1.2f;
  private const float SecondsBeforeRestart = 1f;
  private const float ForcedMovementStrength = 20;

  private GameObject player;
  private PlayerController playerController;
  private SpriteMask spriteMask;
  private SpriteRenderer spriteRenderer;

  void Start()
  {
    player = GameManager.instance.Player;
    playerController = player.GetComponent<PlayerController>();
    spriteMask = GetComponent<SpriteMask>();
    spriteRenderer = GetComponent<SpriteRenderer>();
    StartCoroutine(ClosePortal());
  }

  void Update()
  {
    Vector2 forcedMovementDirection = transform.position - player.transform.position;
    playerController.AddForcedMovement(forcedMovementDirection * ForcedMovementStrength);
  }

  void LateUpdate()
  {
    spriteMask.sprite = spriteRenderer.sprite;
  }

  private IEnumerator ClosePortal()
  {
    yield return new WaitForSeconds(SecondsBeforeClose);
    gameObject.GetComponent<Animator>().SetTrigger("disappear");

    playerController.SetAffectedBySpriteMask(); // Make player "disappear" inside the portal

    yield return new WaitForSeconds(SecondsBeforeRestart);
    SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Restart
  }
}
