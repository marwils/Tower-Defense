using UnityEngine;

/// <summary>
/// Attach this script to an Animator state to automatically disable the GameObject
/// when the state exits. Useful for cleaning up objects after an animation completes,
/// such as removing effects or temporary objects from the scene.
/// </summary>
public class DisableOnStateExit : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SetActive(false);
    }
}