using UnityEngine;

/*
 * Set this script on an Animator state to automatically disable the GameObject
 * when the state exits. This is useful for cleanup after animations that
 * should result in the GameObject being removed from the scene.
 */
public class DisableOnStateExit : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SetActive(false);
    }
}