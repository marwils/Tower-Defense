using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace LevelSystem
{
    [Serializable]
    public abstract class TargetStrategy
    {
        // private static readonly TargetStrategy _nearest = new NearestTargetStrategy();
        // private static readonly TargetStrategy _farthest = new FarthestTargetStrategy();
        // private static readonly TargetStrategy _weakest = new WeakestTargetStrategy();
        // private static readonly TargetStrategy _strongest = new StrongestTargetStrategy();
        // private static readonly TargetStrategy _random = new RandomTargetStrategy();

        // public static TargetStrategy Nearest => _nearest;
        // public static TargetStrategy Farthest => _farthest;
        // public static TargetStrategy Weakest => _weakest;
        // public static TargetStrategy Strongest => _strongest;
        // public static TargetStrategy Random => _random;

        // public abstract CombatTargetBehaviour FindTarget(Transform weaponTransform, List<CombatTargetBehaviour> aimTargets, float range = 5f);
    }

    // [Serializable]
    // public class NearestTargetStrategy : TargetStrategy
    // {
    //     public override CombatTargetBehaviour FindTarget(Transform weaponTransform, List<CombatTargetBehaviour> aimTargets, float range)
    //     {
    //         if (aimTargets == null || aimTargets.Count == 0) return null;

    //         CombatTargetBehaviour nearest = null;
    //         float minDistance = float.MaxValue;

    //         foreach (var aimTarget in aimTargets)
    //         {
    //             if (aimTarget == null) continue;

    //             float distance = Vector3.Distance(weaponTransform.position, aimTarget.transform.position);
    //             if (distance < minDistance && distance <= range)
    //             {
    //                 minDistance = distance;
    //                 nearest = aimTarget;
    //             }
    //         }

    //         return nearest;
    //     }
    // }

    // [Serializable]
    // public class FarthestTargetStrategy : TargetStrategy
    // {
    //     public override CombatTargetBehaviour FindTarget(Transform weaponTransform, List<CombatTargetBehaviour> aimTargets, float range)
    //     {
    //         if (aimTargets == null || aimTargets.Count == 0) return null;

    //         CombatTargetBehaviour farthest = null;
    //         float maxDistance = float.MinValue;

    //         foreach (var aimTarget in aimTargets)
    //         {
    //             if (aimTarget == null) continue;

    //             float distance = Vector3.Distance(weaponTransform.position, aimTarget.transform.position);
    //             if (distance > maxDistance && distance <= range)
    //             {
    //                 maxDistance = distance;
    //                 farthest = aimTarget;
    //             }
    //         }

    //         return farthest;
    //     }
    // }

    // [Serializable]
    // public class WeakestTargetStrategy : TargetStrategy
    // {
    //     public override CombatTargetBehaviour FindTarget(Transform weaponTransform, List<CombatTargetBehaviour> aimTargets, float range)
    //     {
    //         if (aimTargets == null || aimTargets.Count == 0) return null;

    //         CombatTargetBehaviour weakest = null;
    //         float minHealth = float.MaxValue;

    //         foreach (var aimTarget in aimTargets)
    //         {
    //             if (aimTarget == null) continue;

    //             float health = aimTarget.Health;
    //             float distance = Vector3.Distance(weaponTransform.position, aimTarget.transform.position);
    //             if (health < minHealth && distance <= range)
    //             {
    //                 minHealth = health;
    //                 weakest = aimTarget;
    //             }
    //         }

    //         return weakest;
    //     }
    // }

    // [Serializable]
    // public class StrongestTargetStrategy : TargetStrategy
    // {
    //     public override CombatTargetBehaviour FindTarget(Transform weaponTransform, List<CombatTargetBehaviour> aimTargets, float range)
    //     {
    //         if (aimTargets == null || aimTargets.Count == 0) return null;

    //         CombatTargetBehaviour strongest = null;
    //         float maxHealth = float.MinValue;

    //         foreach (var aimTarget in aimTargets)
    //         {
    //             if (aimTarget == null) continue;

    //             float health = aimTarget.Health;
    //             float distance = Vector3.Distance(weaponTransform.position, aimTarget.transform.position);
    //             if (health > maxHealth && distance <= range)
    //             {
    //                 maxHealth = health;
    //                 strongest = aimTarget;
    //             }
    //         }

    //         return strongest;
    //     }
    // }

    // [Serializable]
    // public class RandomTargetStrategy : TargetStrategy
    // {
    //     public override CombatTargetBehaviour FindTarget(Transform weaponTransform, List<CombatTargetBehaviour> aimTargets, float range)
    //     {
    //         if (aimTargets == null || aimTargets.Count == 0) return null;

    //         List<CombatTargetBehaviour> validTargets = aimTargets.Where(aimTarget =>
    //             aimTarget != null && Vector3.Distance(weaponTransform.position, aimTarget.transform.position) <= range).ToList();

    //         if (validTargets.Count == 0) return null;

    //         return validTargets[UnityEngine.Random.Range(0, validTargets.Count)];
    //     }
    // }
}