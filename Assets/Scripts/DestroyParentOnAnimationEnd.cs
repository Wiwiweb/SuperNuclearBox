using UnityEngine;

public class DestroyParentOnAnimationEnd : StateMachineBehaviour
{
  override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    Destroy(animator.gameObject.transform.parent.gameObject);
  }
}
