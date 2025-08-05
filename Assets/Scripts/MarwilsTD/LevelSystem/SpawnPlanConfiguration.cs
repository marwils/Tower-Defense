using System;
using System.Collections;
using System.Collections.Generic;
using MarwilsTD.Helper;
using UnityEngine;

namespace MarwilsTD.LevelSystem
{
    [Serializable]
    public class SpawnPlanConfiguration : WaveElementConfiguration
    {
        [SerializeField]
        [Tooltip("List of route spawns that will be executed in parallel")]
        private List<SequenceConfiguration> _spawnPlanSequences = new();
        public IReadOnlyList<SequenceConfiguration> Sequences => _spawnPlanSequences;

        private bool _isRunning;

        public override IEnumerator Run()
        {
            _isRunning = true;

            if (!ValidateSettings())
            {
                _isRunning = false;
                yield break;
            }

            var runningCoroutines = new List<Coroutine>();

            foreach (var routeSpawn in _spawnPlanSequences)
            {
                if (routeSpawn != null)
                {
                    var coroutine = CoroutineRunner.Start(ExecuteSpawnPlan(routeSpawn));
                    runningCoroutines.Add(coroutine);
                }
            }

            yield return new WaitUntil(() => CoroutineRunner.AllCoroutinesFinished(runningCoroutines));

            _isRunning = false;
        }

        private IEnumerator ExecuteSpawnPlan(SequenceConfiguration routeSpawn)
        {
            yield return routeSpawn.Run();
        }

        private bool ValidateSettings()
        {
            if (_spawnPlanSequences == null || _spawnPlanSequences.Count == 0)
            {
                Debug.LogWarning($"Spawn Plan <{name}> must have at least one RouteSpawn defined.");
                return false;
            }
            return true;
        }

        protected override float GetDuration()
        {
            if (_spawnPlanSequences == null || _spawnPlanSequences.Count == 0)
            {
                return 0f;
            }

            float maxDuration = 0f;
            foreach (var routeSpawn in _spawnPlanSequences)
            {
                if (routeSpawn != null)
                {
                    maxDuration = Mathf.Max(maxDuration, routeSpawn.Duration);
                }
            }
            return maxDuration;
        }

        protected override bool GetIsRunning()
        {
            return _isRunning;
        }
    }
}
