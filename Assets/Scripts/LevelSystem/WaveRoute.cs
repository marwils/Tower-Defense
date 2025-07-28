using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace LevelSystem
{
    [CreateAssetMenu(fileName = "WaveRoute", menuName = "Game/Wave/Route")]
    [Serializable]
    public class WaveRoute : ScriptableObject
    {
        [SerializeField] private string _spawnPointId;
        [SerializeField] private string _targetPointId;

        [SerializeField]
        [Tooltip("The sequence elements defining what and when to spawn")]
        private List<AbstractSequenceElement> _sequenceElements = new();

        public string SpawnPointId => _spawnPointId;
        public string TargetPointId => _targetPointId;
        public IReadOnlyList<AbstractSequenceElement> SequenceElements => _sequenceElements;

        // Runtime Properties
        public Transform SpawnTransform => RouteRegistry.GetSpawnPoint(_spawnPointId);
        public Transform TargetTransform => RouteRegistry.GetTargetPoint(_targetPointId);

        // Validation
        public bool IsValid => SpawnTransform != null && TargetTransform != null;

        public IEnumerator StartRoute()
        {
            Debug.Log($"Starting route: {name}");
            foreach (var element in _sequenceElements)
            {
                if (element == null)
                {
                    Debug.LogError($"Null element in route '{name}'");
                    continue;
                }
                if (element is SequenceSpawner spawner)
                {
                    spawner.SpawnTransform = SpawnTransform;
                    spawner.TargetTransform = TargetTransform;
                }
                yield return element.Run();
            }
        }
    }
}