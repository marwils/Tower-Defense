using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CrystalAnimator : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void StartDestroy()
    {
        _animator.SetTrigger("Destroy");
    }
}
