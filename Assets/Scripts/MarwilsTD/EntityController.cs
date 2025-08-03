using UnityEngine;

namespace MarwilsTD
{
    using LevelSystem;

    public abstract class EntityController : MonoBehaviour
    {
        [SerializeField]
        protected EntityConfiguration _configuration;
        public EntityConfiguration Configuration { get { return GetConfiguration(); } set { SetConfiguration(value); } }

        [Header("Stats (Runtime Properties)")]

        [SerializeField]
        private bool _canTakeDamage;
        public bool CanTakeDamage => _canTakeDamage;

        [SerializeField]
        private bool _canBeHealed;
        public bool CanBeHealed => _canBeHealed;

        [SerializeField]
        private float _health;
        public float Health => _health;
        public bool IsAlive { get { return _health > 0; } }

        [SerializeField]
        private float _shield;
        public float Shield => _shield;
        public bool HasShield { get { return _shield > 0; } }

        [SerializeField]
        private bool _canMove;
        public bool CanMove => _canMove;

        [SerializeField]
        private float _speed;
        public float Speed => _speed;

        [SerializeField]
        private float _maxHealth;
        public float MaxHealth { get { return _maxHealth; } set { SetMaxHealth(value); } }

        [Header("Weapon (Runtime Properties)")]

        [SerializeField]
        protected WeaponController _weaponController;
        public WeaponController WeaponController => _weaponController;
        public bool HasWeapon { get { return _weaponController != null; } }

        protected virtual void Awake()
        {
            if (_configuration == null)
            {
                Debug.LogWarning($"Entity configuration not assigned in <{gameObject.name}>.");
                return;
            }

            InitializeEntity();
        }

        public virtual EntityConfiguration GetConfiguration()
        {
            return _configuration;
        }

        protected virtual void SetConfiguration(EntityConfiguration entityConfiguration)
        {
            if (entityConfiguration == null)
            {
                Debug.LogWarning($"Entity configuration cannot be null in <{gameObject.name}>.");
                return;
            }

            _configuration = entityConfiguration;
            InitializeEntity();
        }

        protected virtual void InitializeEntity()
        {
            _canTakeDamage = _configuration.CanTakeDamage;
            _canBeHealed = _configuration.CanBeHealed;
            _health = _configuration.Health;
            _shield = _configuration.Shield;
            _canMove = _configuration.CanMove;
            _speed = _configuration.Speed;

            _maxHealth = _health;
        }

        public void TakeDamage(float amount, bool ignoreShield = false, float shieldDamageFactor = 1f)
        {
            if (_canTakeDamage)
            {
                if (ignoreShield || _shield <= 0)
                {
                    _health -= amount;
                }
                else
                {
                    _shield -= amount * shieldDamageFactor;
                    if (_shield < 0)
                    {
                        _health += _shield;
                        _shield = 0;
                    }
                }
            }
        }

        public void Heal(float amount)
        {
            if (_canBeHealed)
            {
                _health += amount;

                if (_health > _configuration.Health)
                {
                    _health = _configuration.Health;
                }
            }
        }

        protected void SetMaxHealth(float maxHealth)
        {
            if (maxHealth >= 0)
            {
                _maxHealth = maxHealth;

                if (_health > _maxHealth)
                {
                    _health = _maxHealth;
                }
            }
        }
    }
}