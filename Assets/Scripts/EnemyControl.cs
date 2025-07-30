using LevelSystem;

using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyControl : MonoBehaviour
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

    void Awake()
    {
        if (_enemySettings == null)
        {
            Debug.LogError("Enemy settings not assigned in EnemyControl.");
            Destroy(this);
            return;
        }

        ResetStats();

        _agent = GetComponent<NavMeshAgent>();
    }

    private void ResetStats()
    {
        _currentHealth = _enemySettings.Health;
        _currentSpeed = _enemySettings.Speed;
    }

    public void SetDestination(Transform destination)
    {
        if (destination != null)
        {
            _destination = destination;
        }
        else
        {
            Debug.LogError("No destination set for the enemy.");
            return;
        }

        _agent.SetDestination(_destination.position);
    }
}
