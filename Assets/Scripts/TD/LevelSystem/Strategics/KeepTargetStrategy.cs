using System;

namespace LevelSystem
{
    [Serializable]
    public abstract class KeepTargetStrategy
    {
        // private static readonly KeepTargetStrategy _always = new AlwaysKeepTargetStrategy();
        // private static readonly KeepTargetStrategy _onAttack = new OnAttackKeepTargetStrategy();
        // private static readonly KeepTargetStrategy _onDamage = new OnDamageKeepTargetStrategy();

        // public static KeepTargetStrategy Always => _always;
        // public static KeepTargetStrategy OnAttack => _onAttack;
        // public static KeepTargetStrategy OnDamage => _onDamage;

        // public abstract bool ShouldKeepTarget(Enemy currentTarget, bool hasAttacked, bool hasTakenDamage);
    }

    // [Serializable]
    // public class AlwaysKeepTargetStrategy : KeepTargetStrategy
    // {
    //     public override bool ShouldKeepTarget(Enemy currentTarget, bool hasAttacked, bool hasTakenDamage)
    //     {
    //         return currentTarget != null;
    //     }
    // }

    // [Serializable]
    // public class OnAttackKeepTargetStrategy : KeepTargetStrategy
    // {
    //     public override bool ShouldKeepTarget(Enemy currentTarget, bool hasAttacked, bool hasTakenDamage)
    //     {
    //         return currentTarget != null && hasAttacked;
    //     }
    // }

    // [Serializable]
    // public class OnDamageKeepTargetStrategy : KeepTargetStrategy
    // {
    //     public override bool ShouldKeepTarget(Enemy currentTarget, bool hasAttacked, bool hasTakenDamage)
    //     {
    //         return currentTarget != null && hasTakenDamage;
    //     }
    // }
}