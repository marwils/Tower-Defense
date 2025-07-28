using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace LevelSystem
{
    [CreateAssetMenu(fileName = "Level", menuName = "Game/Level")]
    public class Level : ScriptableObject
    {
        [SerializeField] private string _title;
        public string Title => _title;

        [SerializeField] private List<Wave> _waves = new();
        public List<Wave> Waves => _waves;

        [SerializeField, HideInInspector]
        private string _lastModifiedBySceneName;
        public string LastModifiedBySceneName => _lastModifiedBySceneName;

        [SerializeField, HideInInspector]
        private bool _blockSerialization = false;
        public bool IsSerializationBlocked => _blockSerialization;

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

        public void StartLevel()
        {
            Debug.Log($"Starting level: {_title}");
            foreach (var wave in _waves)
            {
                wave.StartWave();
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
                Debug.LogWarning($"Serialization blocked for Level '{name}' due to scene context mismatch. Please update scene context first.");
            }
        }

        public void OnAfterDeserialize()
        {
            if (IsInDifferentScene())
            {
                _blockSerialization = true;
            }
        }
#endif
    }
}
