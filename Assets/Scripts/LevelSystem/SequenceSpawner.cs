using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace LevelSystem
{
    [CreateAssetMenu(fileName = "SequenceSpawner", menuName = "Game/Sequence/Spawner")]
    public class SequenceSpawner : AbstractSequenceElement, IRouteAware
    {
        [SerializeField]
        [Tooltip("List of enemies to spawn")]
        private List<EnemyControl> _enemies = new();
        public IReadOnlyList<EnemyControl> Enemies => _enemies;

        public Transform SpawnTransform { get; set; }
        public Transform TargetTransform { get; set; }

        [SerializeField]
        [Tooltip("Randomize the spawn order?")]
        private bool _shuffleSpawnOrder;
        public bool ShuffleSpawnOrder => _shuffleSpawnOrder;

        [SerializeField]
        [Min(1)]
        [Tooltip("Total number of enemies to spawn")]
        private int _spawnAmount = 1;
        public int SpawnAmount => _spawnAmount;

        [SerializeField]
        [Range(0.1f, 5f)]
        [Tooltip("Time between spawns in seconds")]
        private float _interval = 0.5f;
        public float Interval => _interval;

        private int _currentIndex;

        private bool _isRunning;

        public override IEnumerator Run()
        {
            _isRunning = true;
            if (!ValidateSettings())
            {
                yield break;
            }

            if (_shuffleSpawnOrder)
            {
                ShuffleEnemies();
            }

            yield return SpawnEnemies();
            _isRunning = false;
        }

        private bool ValidateSettings()
        {
            if (_enemies == null || _enemies.Count == 0)
            {
                Debug.LogError($"Enemy prefabs list is empty in {name}");
                return false;
            }
            return true;
        }

        private IEnumerator SpawnEnemies()
        {
            var wait = new WaitForSeconds(_interval);
            var lastIndex = _spawnAmount - 1;

            for (int i = 0; i < _spawnAmount; i++)
            {
                SpawnEnemy();
                if (i < lastIndex)
                {
                    yield return wait;
                }
            }
        }

        private void SpawnEnemy()
        {
            SpawnTransform.gameObject.GetComponent<SpawnPoint>().DoSpawn(
                _enemies[_currentIndex],
                TargetTransform
            );

            _currentIndex = (_currentIndex + 1) % _enemies.Count;
        }

        private void ShuffleEnemies()
        {
            var count = _enemies.Count;
            var last = count - 1;
            for (var i = 0; i < last; ++i)
            {
                var r = UnityEngine.Random.Range(i, count);
                (_enemies[i], _enemies[r]) = (_enemies[r], _enemies[i]);
            }
        }

        private void OnValidate()
        {
            if (_enemies == null || _enemies.Count == 0)
            {
                Debug.LogWarning($"Enemy prefabs list is empty in {name}");
            }
            _spawnAmount = Mathf.Max(1, _spawnAmount);
        }

        protected override float GetDuration()
        {
            return _spawnAmount * _interval + (_spawnAmount > 0 ? _interval : 0);
        }

        protected override bool GetIsRunning()
        {
            return _isRunning;
        }
    }
}