using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [SerializeField]
    public Transform _target;

    [SerializeField]
    public Weapon _weapon;

    private NavMeshAgent _agent;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        Invoke("GoTo", 1f);
    }

    void GoTo()
    {
        if (_target != null)
        {
            _agent.SetDestination(_target.position);
        }
    }
}
