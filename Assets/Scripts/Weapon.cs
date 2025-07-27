using LevelSystem;

using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField]
    private LevelSystem.Weapon _metadata;

    [SerializeField]
    private AbstractTargetStrategy _targetingStrategy;

    [SerializeField]
    private KeepTargetStrategy _keepTargetStrategy;

    [SerializeField]
    private Transform _turret;

    public abstract void Attack();

    public abstract void SeekTarget();

    public abstract void Reload();

    private Transform _target;
    public Transform Target { get => _target; }

    protected void FindTarget()
    {
        if (_target == null)
        {
            _target = GameObject.FindGameObjectWithTag(TargetTag)?.transform;
            if (_target == null)
            {
                Debug.LogError("No target found with tag: " + TargetTag);
            }
        }
    }

    private const string TargetTag = "Enemy";

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        if (_metadata != null)
        {
            Gizmos.DrawWireSphere(transform.position, _metadata.Range);
        }
    }
}
