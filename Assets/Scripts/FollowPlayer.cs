using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

  public GameObject player;

  [SerializeField]
  private BoxArrowController boxArrowController;

  private float height = 3;

  void LateUpdate()
  {
    transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -height);
    boxArrowController.UpdatePosition();
  }
}
