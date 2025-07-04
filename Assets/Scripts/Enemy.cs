using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    public Transform Target { get; set; }

    private NavMeshAgent _agent;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        Invoke("GoTo", 1f);
    }

    void GoTo()
    {
        _agent.SetDestination(Target.position);
    }
}
