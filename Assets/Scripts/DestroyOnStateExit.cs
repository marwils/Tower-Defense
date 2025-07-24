using UnityEngine;

/*
 * When an animation state has ended it destroys its game object.
 */
public class DestroyOnStateExit : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Destroy(animator.gameObject);
    }
}