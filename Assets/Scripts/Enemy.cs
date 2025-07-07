using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Transform _target;

    [SerializeField]
    private Weapon _weapon;

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
            Debug.Log("Go to " + _target.name);
            _agent.SetDestination(_target.position);
        }
    }
}
