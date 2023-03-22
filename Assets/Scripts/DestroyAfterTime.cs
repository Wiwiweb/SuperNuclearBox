using UnityEngine;

public class DestroyAfterTime : MonoBehaviour 
{
    [SerializeField]
    float timeToLive = 1;

    void Start() 
	{
        Destroy(gameObject, timeToLive);
    }
}
