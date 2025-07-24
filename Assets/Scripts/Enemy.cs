using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    public Transform Target { get; set; }

    [SerializeField]
    private Weapon _weapon;

    private NavMeshAgent _agent;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    public void FindTarget()
    {
        if (Target != null)
        {
            _agent.SetDestination(Target.position);
        }
    }
}
