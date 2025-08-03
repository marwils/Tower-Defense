using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CrystalAnimator : MonoBehaviour
{
    private const string CollectTrigger = "Collect";

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogWarning($"Animator component is not assigned in <{gameObject.name}>.");
            Destroy(this);
            return;
        }
    }

    public void StartCollect()
    {
        StartCollect(0f);
    }

    public void StartCollect(float timeOffset)
    {
        Invoke(nameof(SetCollectTrigger), timeOffset);
    }

    private void SetCollectTrigger()
    {
        SetTrigger(CollectTrigger);
    }

    private void SetTrigger(string triggerName)
    {
        _animator.SetTrigger(triggerName);
    }
}
