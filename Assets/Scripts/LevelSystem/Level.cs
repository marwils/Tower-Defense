using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace LevelSystem
{
    [CreateAssetMenu(fileName = "New Level", menuName = "Game/Level")]
    [System.Serializable]
    /// <summary>
    /// Represents a level in the game, containing routes and waves.
    /// </summary>
    public class Level : TimeElement
    {
        [SerializeField]
        private string _title = "New Level";
        public string Title => _title;

        [SerializeField]
        [Tooltip("List of routes used in the level.")]
        private List<Route> _routes = new();
        public IReadOnlyList<Route> Routes => _routes;

        [SerializeField]
        [Tooltip("List of waves in the level.")]
        private List<Wave> _waves = new();
        public List<Wave> Waves => _waves;

        [SerializeField, HideInInspector]
        private string _lastModifiedBySceneName;
        public string LastModifiedBySceneName => _lastModifiedBySceneName;

        [SerializeField, HideInInspector]
        private bool _blockSerialization = false;
        public bool IsSerializationBlocked => _blockSerialization;

        public void StartLevel()
        {
            Debug.Log($"Starting level: {_title}");
            foreach (var wave in _waves)
            {
                wave.StartWave();
            }
        }

        public bool IsInDifferentScene()
        {
            if (string.IsNullOrEmpty(_lastModifiedBySceneName))
                return false;

            var currentScene = SceneManager.GetActiveScene();
            bool isDifferent = !string.IsNullOrEmpty(currentScene.name) &&
                               currentScene.name != _lastModifiedBySceneName;

            if (isDifferent)
            {
                _blockSerialization = true;
            }

            return isDifferent;
        }

        public void UpdateSceneContext()
        {
            var currentScene = SceneManager.GetActiveScene();
            if (!string.IsNullOrEmpty(currentScene.name))
            {
                _lastModifiedBySceneName = currentScene.name;
                _blockSerialization = false;
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }
        }

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(_title))
            {
                _title = name;
            }

            if (_waves == null)
            {
                _waves = new List<Wave>();
            }

            _waves.RemoveAll(element => element == null);

            var currentScene = SceneManager.GetActiveScene();
            if (!string.IsNullOrEmpty(_lastModifiedBySceneName) &&
                !string.IsNullOrEmpty(currentScene.name) &&
                currentScene.name != _lastModifiedBySceneName)
            {
                _blockSerialization = true;
            }
        }

#if UNITY_EDITOR
        public void OnBeforeSerialize()
        {
            if (_blockSerialization)
            {
                return;
            }
        }

        public void OnAfterDeserialize()
        {
            if (IsInDifferentScene())
            {
                _blockSerialization = true;
            }
        }

        protected override float GetDuration()
        {
            float totalDuration = 0f;
            foreach (var wave in _waves)
            {
                totalDuration += wave.Duration;
            }
            return totalDuration;
        }

        protected override bool GetIsRunning()
        {
            foreach (var wave in _waves)
            {
                if (wave.IsRunning)
                {
                    return true;
                }
            }
            return false;
        }
#endif
    }
}
