# Unity C# Style Guide

## Purpose
This guide follows modern C#/.NET conventions, adapted for Unity. The goal is clear, consistent, and maintainable code.

## 1. Naming Conventions

| Element Type             | Style           | Example                         |
|--------------------------|------------------|----------------------------------|
| Classes, Structs         | PascalCase       | `PlayerController`, `GameData`  |
| Interfaces               | IPascalCase      | `IDamageable`, `IMovable`       |
| Methods                  | PascalCase       | `TakeDamage()`, `MovePlayer()`  |
| Properties               | PascalCase       | `Health`, `IsAlive`             |
| Local Variables          | camelCase        | `currentSpeed`, `isGrounded`    |
| Parameters               | camelCase        | `int damageAmount`              |
| Private Fields           | _camelCase       | `_health`, `_target`            |
| [SerializeField] Fields  | _camelCase       | `[SerializeField] private float _speed;` |
| Constants (`const`)      | PascalCase       | `MaxHealth`, `Gravity`          |
| Readonly Fields          | _camelCase       | `private readonly Vector3 _spawnPoint;` |
| Static Fields            | _camelCase (private), PascalCase (public) | `_instance`, `GameManager.Instance` |
| Enums                    | PascalCase       | `GameState`, `MovementType`     |
| Enum Values              | PascalCase       | `Idle`, `Running`, `Jumping`    |
| Events                   | PascalCase       | `OnDeath`, `OnRespawn`          |

## 2. Modifier Order

Use the recommended C# modifier order:

```
[Attribute] access static abstract/virtual/override readonly const type name
```

### Examples:
```csharp
public static GameManager Instance;
private readonly float _gravity = 9.81f;
public override void Update() { ... }
private const int MaxEnemies = 10;
```

## 3. File and Class Names

- Each class should be in its own `.cs` file.
- File name should match the class name (`PlayerController.cs` → `public class PlayerController {}`)

## 4. Unity-specific Methods

These methods must be named exactly this way to be recognized by Unity:

```csharp
Awake()
Start()
Update()
FixedUpdate()
LateUpdate()
OnTriggerEnter(Collider other)
OnCollisionEnter(Collision collision)
```

## 5. Class Member Ordering

1. Constants
2. Serialized Fields
3. Private Fields
4. Public Properties
5. Events & Delegates
6. Unity Lifecycle Methods (Awake, Start, etc.)
7. Public Methods
8. Private Methods
9. Helper Methods / Coroutines / Callbacks

## 6. Namespaces

- Use namespaces for non-MonoBehaviour classes.
- Format: `Company.Product.Module` or `Game.System.Component`

## 7. Additional Guidelines

- No Hungarian Notation (no `strName`, `iCount`, etc.)
- Do not use `ALL_CAPS` for constants
- Prefer `[SerializeField] private` over public fields
- Make methods public only when they are part of the intended API
