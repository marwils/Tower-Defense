using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField]
    private float _range;

    public abstract void Attack();

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _range);
    }
}
