using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

  [SerializeField]
  private GameObject player;

  private float height = 3;

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -height);
  }
}
