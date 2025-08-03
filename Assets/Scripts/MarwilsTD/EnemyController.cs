using UnityEngine;
using UnityEngine.AI;

namespace MarwilsTD
{
    using LevelSystem;

    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyController : EntityController
    {
        [Header("Enemy Configuration")]

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
            _agent = GetComponent<NavMeshAgent>();

            base.Awake();
        }

        protected override void InitializeEntity()
        {
            base.InitializeEntity();

            _currentHealth = GetConfiguration().Health;
            _agent.speed = GetConfiguration().Speed;
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
                Debug.LogWarning($"No destination set for the enemy in <{gameObject.name}>.");
            }
        }

        public new EnemyConfiguration GetConfiguration()
        {
            return _configuration as EnemyConfiguration;
        }

        public void SetConfiguration(EnemyConfiguration enemyConfiguration)
        {
            if (enemyConfiguration == null)
            {
                Debug.LogWarning($"Enemy configuration cannot be null in <{gameObject.name}>.");
                return;
            }

            _configuration = enemyConfiguration;
            InitializeEntity();
        }
    }
}