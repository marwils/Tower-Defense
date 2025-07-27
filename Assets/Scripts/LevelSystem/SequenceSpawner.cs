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
        [Tooltip("List of enemy prefabs to spawn")]
        private List<global::Enemy> _enemyPrefabs = new();
        public IReadOnlyList<global::Enemy> EnemyPrefabs => _enemyPrefabs;

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

        protected override IEnumerator Coroutine()
        {
            if (!ValidateSettings())
            {
                yield break;
            }

            if (_shuffleSpawnOrder)
            {
                ShuffleEnemies();
            }

            yield return SpawnEnemies();
        }

        private bool ValidateSettings()
        {
            if (_enemyPrefabs == null || _enemyPrefabs.Count == 0)
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
            var prefab = _enemyPrefabs[_currentIndex];
            var instance = Instantiate(prefab, SpawnTransform.position, prefab.transform.rotation);
            instance.GetComponent<global::Enemy>().Destination = TargetTransform.position;

            _currentIndex = (_currentIndex + 1) % _enemyPrefabs.Count;
        }

        private void ShuffleEnemies()
        {
            var count = _enemyPrefabs.Count;
            var last = count - 1;
            for (var i = 0; i < last; ++i)
            {
                var r = UnityEngine.Random.Range(i, count);
                (_enemyPrefabs[i], _enemyPrefabs[r]) = (_enemyPrefabs[r], _enemyPrefabs[i]);
            }
        }

        private void OnValidate()
        {
            if (_enemyPrefabs == null || _enemyPrefabs.Count == 0)
            {
                Debug.LogWarning($"Enemy prefabs list is empty in {name}");
            }
            _spawnAmount = Mathf.Max(1, _spawnAmount);
        }
    }
}