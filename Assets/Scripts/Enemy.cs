using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _health = 100f;
    public float Health { get { return _health; } }

    [SerializeField]
    private Weapon _weapon;
    public Weapon Weapon { get { return _weapon; } }

    public Transform Destination { get; set; }

    private Transform _target;

    private NavMeshAgent _agent;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    public void FindDestination()
    {
        if (Destination == null)
        {
            Debug.LogError("Destination is not set for the enemy.");
            return;
        }

        _agent.SetDestination(Destination.position);
    }
}
