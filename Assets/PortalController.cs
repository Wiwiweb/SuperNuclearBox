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

  // Start is called before the first frame update
  void Start()
  {
    player = GameManager.instance.Player;
    playerController = player.GetComponent<PlayerController>();
    StartCoroutine(ClosePortal());
  }

  // Update is called once per frame
  void Update()
  {
    Vector2 forcedMovementDirection = transform.position - player.transform.position;
    playerController.AddForcedMovement(forcedMovementDirection * ForcedMovementStrength);
  }

  private IEnumerator ClosePortal()
  {
    yield return new WaitForSeconds(SecondsBeforeClose);
    gameObject.GetComponent<Animator>().SetTrigger("disappear");

    yield return new WaitForSeconds(SecondsBeforeRestart);
    SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Restart
  }
}
