using System.Collections.Generic;
using UnityEngine;

namespace MarwilsTD
{
    public static class RouteRegistry
    {
        private static Dictionary<string, Transform> _spawnPoints = new();
        private static Dictionary<string, Transform> _targetPoints = new();

        public static void RegisterSpawnPoint(Transform transform)
        {
            if (transform != null)
                _spawnPoints[transform.name] = transform;
        }

        public static void UnregisterSpawnPoint(Transform transform)
        {
            if (transform != null)
                _spawnPoints.Remove(transform.name);
        }

        public static void RegisterTargetPoint(Transform transform)
        {
            if (transform != null)
                _targetPoints[transform.name] = transform;
        }

        public static void UnregisterTargetPoint(Transform transform)
        {
            if (transform != null)
                _targetPoints.Remove(transform.name);
        }

        public static Transform GetSpawnPoint(string id)
        {
            return _spawnPoints.TryGetValue(id, out var transform) ? transform : null;
        }

        public static Transform GetTargetPoint(string id)
        {
            return _targetPoints.TryGetValue(id, out var transform) ? transform : null;
        }

        public static IEnumerable<string> GetAllSpawnPointNames()
        {
            return _spawnPoints.Keys;
        }

        public static IEnumerable<string> GetAllTargetPointNames()
        {
            return _targetPoints.Keys;
        }
    }
}
