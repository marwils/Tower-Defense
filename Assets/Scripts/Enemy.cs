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
        Debug.Log("Finding target...");
        if (Target != null)
        {
            Debug.Log("Go to " + Target.name);
            _agent.SetDestination(Target.position);
        }
    }
}
