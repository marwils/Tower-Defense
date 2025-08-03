using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MarwilsTD.LevelSystem
{
    public class EnemySpawnerConfiguration : SequenceElementConfiguration
    {
        [SerializeField]
        [Tooltip("List of enemies to spawn")]
        private List<EnemyController> _enemies = new();
        public IReadOnlyList<EnemyController> Enemies => _enemies;

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

        public Transform SpawnTransform => _routeProvider?.Route?.SpawnTransform;
        public Transform TargetTransform => _routeProvider?.Route?.TargetTransform;

        public override IEnumerator Run()
        {
            _isRunning = true;
            if (!ValidateSettings())
            {
                _isRunning = false;
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
                Debug.LogWarning($"Enemy prefabs list is empty in <{name}>.");
                return false;
            }

            if (_routeProvider == null)
            {
                Debug.LogWarning($"No route provider set for <{name}>.");
                return false;
            }

            if (SpawnTransform == null || TargetTransform == null)
            {
                Debug.LogWarning($"Spawn or target transform is null in <{name}>.");
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
            var request = new EnemySpawnRequest(
                _enemies[_currentIndex],
                SpawnTransform,
                TargetTransform,
                this
            );

            EnemySpawnEvent.RequestSpawn(request);

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
                Debug.LogWarning($"Enemy prefabs list is empty in <{name}>.");
            }
            _spawnAmount = Mathf.Max(1, _spawnAmount);
        }

        protected override float GetDuration()
        {
            return _spawnAmount > 0 ? (_spawnAmount - 1) * _interval : 0f;
        }

        protected override bool GetIsRunning()
        {
            return _isRunning;
        }
    }
}