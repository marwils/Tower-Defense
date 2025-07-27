using System;
using System.Collections.Generic;

using UnityEngine;

namespace LevelSystem
{
    [CreateAssetMenu(fileName = "WaveRoute", menuName = "Game/Wave/Route")]
    [Serializable]
    public class WaveRoute : ScriptableObject
    {
        [SerializeField]
        [Tooltip("The spawn point where enemies will be created")]
        private Transform _spawnPoint;
        public Transform SpawnPoint => _spawnPoint;

        [SerializeField]
        [Tooltip("The target point where enemies will move to")]
        private Transform _targetPoint;
        public Transform TargetPoint => _targetPoint;

        [SerializeField]
        [Tooltip("The sequence elements defining what and when to spawn")]
        private List<AbstractSequenceElement> _sequenceElements = new();
        public IReadOnlyList<AbstractSequenceElement> SequenceElements => _sequenceElements;

        private void OnValidate()
        {
            if (_sequenceElements == null)
            {
                _sequenceElements = new List<AbstractSequenceElement>();
            }

            _sequenceElements.RemoveAll(element => element == null);
        }
    }
}