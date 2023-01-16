using UnityEngine;

public class TestController : MonoBehaviour
{
    private float speed = 5f;

    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed);
    }

    //Upon collision with another GameObject, this GameObject will reverse direction
    private void OnTriggerEnter2D(Collider2D other)
    {
        speed = speed * -1;
    }
}