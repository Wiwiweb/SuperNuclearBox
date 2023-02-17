using UnityEngine;

public class DestroyOnAnimationEnd : MonoBehaviour 
{
    void DestroyParent() 
	{
        Destroy(gameObject.transform.parent.gameObject);
    }
}
