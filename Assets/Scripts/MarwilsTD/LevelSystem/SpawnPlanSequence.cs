using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MarwilsTD.LevelSystem
{
    [Serializable]
    public class SpawnPlanSequence : Runner, IRouteProvider
    {
        [SerializeField]
        [Tooltip("The routes that define the spawn points and target points for the wave sequence")]
        private Route _route;
        public Route Route => _route;

        [SerializeField]
        private List<SequenceElement> _sequenceElements = new();
        public IReadOnlyList<SequenceElement> SequenceElements => _sequenceElements;

        public Transform SpawnTransform => _route?.SpawnTransform;
        public Transform TargetTransform => _route?.TargetTransform;

        private bool _isRunning;

        public override IEnumerator Run()
        {
            _isRunning = true;
            if (!ValidateSettings())
            {
                yield break;
            }
            if (!InitializeElements())
            {
                yield break;
            }

            yield return RunRoute();
            _isRunning = false;
        }

        private bool ValidateSettings()
        {
            if (_route == null)
            {
                Debug.LogWarning($"No route defined in {name}");
                return false;
            }

            Debug.Log($"Route {_route.name} is starting...");
            if (!_route.IsValid)
            {
                Debug.LogError($"Invalid route in {name}: {_route.name}");
                return false;
            }

            if (_sequenceElements == null || _sequenceElements.Count == 0)
            {
                Debug.LogError($"No sequence elements defined in {name}");
                return false;
            }

            return true;
        }

        private bool InitializeElements()
        {
            bool isValid = true;
            for (int i = 0; i < _sequenceElements.Count; i++)
            {
                var element = _sequenceElements[i];
                if (element == null)
                {
                    Debug.LogError($"Null sequence element found in {name} at index {i}");
                    isValid = false;
                    continue;
                }
                else
                {
                    element.Initialize(this);
                }
            }
            return isValid;
        }

        private IEnumerator RunRoute()
        {
            Debug.Log($"Starting SpawnPlan: {name}");

            foreach (var element in _sequenceElements)
            {
                if (element == null) continue;
                yield return element.Run();
            }
        }

        protected override float GetDuration()
        {
            if (_sequenceElements == null || _sequenceElements.Count == 0)
            {
                return 0f;
            }
            float totalDuration = 0f;
            foreach (var element in _sequenceElements)
            {
                totalDuration += element.Duration;
            }
            return totalDuration;
        }

        protected override bool GetIsRunning()
        {
            return _isRunning;
        }
    }
}