using System;
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
        public Transform SpawnPoint => SpawnPointRegistry.GetSpawnPoint(_spawnPointId);
        public Transform TargetPoint => SpawnPointRegistry.GetTargetPoint(_targetPointId);

        // Validation
        public bool IsValid => SpawnPoint != null && TargetPoint != null;

        void Start()
        {
            Debug.Log(SpawnPoint?.gameObject.name);
            Debug.Log(TargetPoint?.gameObject.name);
        }
    }
}