using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CrystalAnimator : MonoBehaviour
{
    Animator _animator;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void StartDestroy()
    {
        _animator.SetTrigger("Destroy");
    }
}
