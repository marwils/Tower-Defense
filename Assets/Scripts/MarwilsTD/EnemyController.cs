using MarwilsTD.LevelSystem;

using UnityEngine;
using UnityEngine.AI;

namespace MarwilsTD
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyController : EntityController
    {
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

            _currentHealth = GetEntitySettings<Enemy>().Health;
            _agent.speed = GetEntitySettings<Enemy>().Speed;
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
            return _entitySettings as Enemy;
        }
    }
}