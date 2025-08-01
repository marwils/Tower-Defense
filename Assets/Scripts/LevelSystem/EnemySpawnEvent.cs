using System;

using UnityEngine;

namespace LevelSystem
{
    public static class EnemySpawnEvent
    {
        public static event Action<EnemySpawnRequest> OnEnemySpawnRequested;

        public static void RequestSpawn(EnemySpawnRequest request)
        {
            OnEnemySpawnRequested?.Invoke(request);
        }
    }

    [Serializable]
    public class EnemySpawnRequest
    {
        public EnemyControl EnemyPrefab { get; set; }
        public Transform SpawnTransform { get; set; }
        public Transform TargetTransform { get; set; }
        public ISequenceElement SourceSequence { get; set; }

        public EnemySpawnRequest(EnemyControl enemyPrefab, Transform spawnTransform, Transform targetTransform, ISequenceElement sourceSequence)
        {
            EnemyPrefab = enemyPrefab;
            SpawnTransform = spawnTransform;
            TargetTransform = targetTransform;
            SourceSequence = sourceSequence;
        }
    }
}