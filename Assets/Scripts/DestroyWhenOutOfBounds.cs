using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWhenOutOfBounds : MonoBehaviour
{
  public float bounds = 20;

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    if (transform.position.x < -bounds || transform.position.x > bounds ||
        transform.position.y < -bounds || transform.position.y > bounds)
    {
      Destroy(gameObject);
    }
  }
}
