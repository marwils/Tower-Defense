using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField]
    private float _range = 5f;

    [SerializeField]
    private Transform _turret;

    [SerializeField]
    private string _targetTag;

    public abstract void Attack();

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _range);
    }
}
