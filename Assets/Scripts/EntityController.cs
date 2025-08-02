using LevelSystem;

using UnityEngine;

public abstract class EntityController : MonoBehaviour
{
    [SerializeField]
    protected Entity _entitySettings;
    public Entity EntitySettings => _entitySettings;

    private bool _canTakeDamage;
    public bool CanTakeDamage => _canTakeDamage;

    private bool _canBeHealed;
    public bool CanBeHealed => _canBeHealed;

    private float _health;
    public float Health => _health;

    private float _shield;
    public float Shield => _shield;

    private bool _canMove;
    public bool CanMove => _canMove;

    private float _speed;
    public float Speed => _speed;

    private float _maxHealth;
    public float MaxHealth => _maxHealth;

    protected virtual void Awake()
    {
        if (_entitySettings == null)
        {
            Debug.LogError("Entity settings not assigned in " + gameObject.name);
            return;
        }

        Initialize();
    }

    protected virtual T GetEntitySettings<T>() where T : Entity
    {
        return _entitySettings as T;
    }

    protected virtual void SetEntitySettings(Entity entitySettings)
    {
        if (entitySettings == null)
        {
            Debug.LogError("Entity settings cannot be null");
            return;
        }

        _entitySettings = entitySettings;
        Initialize();
    }

    protected virtual void Initialize()
    {
        _canTakeDamage = _entitySettings.CanTakeDamage;
        _canBeHealed = _entitySettings.CanBeHealed;
        _health = _entitySettings.Health;
        _shield = _entitySettings.Shield;
        _canMove = _entitySettings.CanMove;
        _speed = _entitySettings.Speed;

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

            if (_health > _entitySettings.Health)
            {
                _health = _entitySettings.Health;
            }
        }
    }

    public bool IsAlive()
    {
        return _health > 0;
    }

    public bool HasShield()
    {
        return _shield > 0;
    }

    public void SetMaxHealth(float maxHealth)
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
