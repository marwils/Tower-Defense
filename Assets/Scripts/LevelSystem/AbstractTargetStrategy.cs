using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace LevelSystem
{
    [Serializable]
    public abstract class AbstractTargetStrategy
    {
        private static readonly AbstractTargetStrategy _nearest = new NearestTargetStrategy();
        private static readonly AbstractTargetStrategy _farthest = new FarthestTargetStrategy();
        private static readonly AbstractTargetStrategy _weakest = new WeakestTargetStrategy();
        private static readonly AbstractTargetStrategy _strongest = new StrongestTargetStrategy();
        private static readonly AbstractTargetStrategy _random = new RandomTargetStrategy();

        public static AbstractTargetStrategy Nearest => _nearest;
        public static AbstractTargetStrategy Farthest => _farthest;
        public static AbstractTargetStrategy Weakest => _weakest;
        public static AbstractTargetStrategy Strongest => _strongest;
        public static AbstractTargetStrategy Random => _random;

        public abstract global::Enemy FindTarget(Transform weaponTransform, List<global::Enemy> enemies, float range = 5f);
    }

    [Serializable]
    public class NearestTargetStrategy : AbstractTargetStrategy
    {
        public override global::Enemy FindTarget(Transform weaponTransform, List<global::Enemy> enemies, float range)
        {
            if (enemies == null || enemies.Count == 0) return null;

            global::Enemy nearest = null;
            float minDistance = float.MaxValue;

            foreach (var enemy in enemies)
            {
                float distance = Vector3.Distance(weaponTransform.position, enemy.transform.position);
                if (distance < minDistance && distance <= range)
                {
                    minDistance = distance;
                    nearest = enemy;
                }
            }

            return nearest;
        }
    }

    // Weitere Strategien analog implementieren
    [Serializable]
    public class FarthestTargetStrategy : AbstractTargetStrategy
    {
        public override global::Enemy FindTarget(Transform weaponTransform, List<global::Enemy> enemies, float range)
        {
            if (enemies == null || enemies.Count == 0) return null;

            global::Enemy farthest = null;
            float maxDistance = float.MinValue;

            foreach (var enemy in enemies)
            {
                float distance = Vector3.Distance(weaponTransform.position, enemy.transform.position);
                if (distance > maxDistance && distance <= range)
                {
                    maxDistance = distance;
                    farthest = enemy;
                }
            }

            return farthest;
        }
    }

    [Serializable]
    public class WeakestTargetStrategy : AbstractTargetStrategy
    {
        public override global::Enemy FindTarget(Transform weaponTransform, List<global::Enemy> enemies, float range)
        {
            if (enemies == null || enemies.Count == 0) return null;

            global::Enemy weakest = null;
            float minHealth = float.MaxValue;

            foreach (var enemy in enemies)
            {
                float health = enemy.Health;
                float distance = Vector3.Distance(weaponTransform.position, enemy.transform.position);
                if (health < minHealth && distance <= range)
                {
                    minHealth = health;
                    weakest = enemy;
                }
            }

            return weakest;
        }
    }

    [Serializable]
    public class StrongestTargetStrategy : AbstractTargetStrategy
    {
        public override global::Enemy FindTarget(Transform weaponTransform, List<global::Enemy> enemies, float range)
        {
            if (enemies == null || enemies.Count == 0) return null;

            global::Enemy strongest = null;
            float maxHealth = float.MinValue;

            foreach (var enemy in enemies)
            {
                float health = enemy.Health;
                float distance = Vector3.Distance(weaponTransform.position, enemy.transform.position);
                if (health > maxHealth && distance <= range)
                {
                    maxHealth = health;
                    strongest = enemy;
                }
            }

            return strongest;
        }
    }

    [Serializable]
    public class RandomTargetStrategy : AbstractTargetStrategy
    {
        public override global::Enemy FindTarget(Transform weaponTransform, List<global::Enemy> enemies, float range)
        {
            if (enemies == null || enemies.Count == 0) return null;

            List<global::Enemy> validTargets = enemies.Where(enemy =>
                Vector3.Distance(weaponTransform.position, enemy.transform.position) <= range).ToList();

            if (validTargets.Count == 0) return null;

            return validTargets[UnityEngine.Random.Range(0, validTargets.Count)];
        }
    }
}