using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CrystalAnimator : MonoBehaviour
{
    private const string DestroyTrigger = "Destroy";

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("Animator component is not assigned.");
            Destroy(this);
            return;
        }
    }

    public void StartDestroy()
    {
        Invoke("SetDestroyTrigger", Random.Range(0.0f, 0.2f));
    }

    private void SetDestroyTrigger()
    {
        SetTrigger(DestroyTrigger);
    }

    private void SetTrigger(string triggerName)
    {
        _animator.SetTrigger(triggerName);
    }
}
