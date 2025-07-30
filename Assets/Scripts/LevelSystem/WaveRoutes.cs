using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace LevelSystem
{
    [CreateAssetMenu(fileName = "WaveRoutes", menuName = "Game/Wave/Routes")]
    [Serializable]
    public class WaveRoutes : AbstractWaveElement
    {
        [SerializeField]
        [Tooltip("List of routes that will execute simultaneously")]
        private List<WaveRoute> _routes = new();
        public IReadOnlyList<WaveRoute> Routes => _routes;

        public override IEnumerator Run()
        {
            if (_routes == null || _routes.Count == 0)
            {
                Debug.LogWarning($"No routes defined in {name}");
                yield break;
            }

            foreach (var route in _routes)
            {
                Debug.Log($"Route {route.name} is starting...");
                if (!route.IsValid)
                {
                    Debug.LogError($"Invalid route in {name}: {route.name}");
                    continue;
                }

                yield return route.StartRoute();
            }
            yield return null;
        }

        public void AddRoute(WaveRoute route)
        {
            if (route == null)
            {
                Debug.LogError($"Cannot add null route to {name}");
                return;
            }

            _routes.Add(route);
            UnityEditor.EditorUtility.SetDirty(this);
        }

        public void RemoveRoute(int index)
        {
            if (index < 0 || index >= _routes.Count)
            {
                Debug.LogError($"Invalid route index {index} for {name}");
                return;
            }

            _routes.RemoveAt(index);
            UnityEditor.EditorUtility.SetDirty(this);
        }

        private void OnValidate()
        {
            if (_routes == null)
            {
                _routes = new List<WaveRoute>();
            }

            _routes.RemoveAll(route => route == null);
        }

        protected override float GetDuration()
        {
            throw new NotImplementedException();
        }

        protected override bool GetIsRunning()
        {
            throw new NotImplementedException();
        }
    }
}