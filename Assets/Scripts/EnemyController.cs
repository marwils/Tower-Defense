using LevelSystem;

using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : EntityController
{
    [SerializeField]
    private Enemy _enemySettings;
    public Enemy EnemySettings => _enemySettings;

    [SerializeField]
    private Transform _destination;
    public Transform Destination => _destination;

    [SerializeField]
    private float _currentHealth;
    public float CurrentHealth => _currentHealth;

    [SerializeField]
    private float _currentSpeed;
    public float CurrentSpeed => _currentSpeed;

    private NavMeshAgent _agent;

    protected override void Awake()
    {
        base.Awake();

        _agent = GetComponent<NavMeshAgent>();
    }

    protected override void Initialize()
    {
        base.Initialize();
        _currentHealth = _enemySettings.Health;
        _agent.speed = _enemySettings.Speed;
    }

    public void SetDestination(Transform destination)
    {
        if (destination != null)
        {
            _destination = destination;
            _agent.SetDestination(_destination.position);
        }
        else
        {
            Debug.LogError("No destination set for the enemy.");
        }
    }

    protected override Enemy GetEntitySettings<Enemy>()
    {
        return _enemySettings as Enemy;
    }
}
