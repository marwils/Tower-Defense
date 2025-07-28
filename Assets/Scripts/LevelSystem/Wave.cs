using System;
using System.Collections;
using System.Collections.Generic;

using Helper;

using UnityEditor;

using UnityEngine;

namespace LevelSystem
{
    [CreateAssetMenu(fileName = "Wave", menuName = "Game/Wave")]
    public class Wave : ScriptableObject
    {
        [SerializeField]
        private string _title = "New Wave";
        public string Title => _title;

        [SerializeField]
        private List<AbstractWaveElement> _waveElements = new();
        public IReadOnlyList<AbstractWaveElement> WaveElements => _waveElements;

        public void AddWaveElement(AbstractWaveElement element)
        {
            if (element == null)
            {
                Debug.LogError($"Cannot add null element to wave '{_title}'");
                return;
            }

            _waveElements.Add(element);
            EditorUtility.SetDirty(this);
        }

        public void RemoveWaveElement(int index)
        {
            if (index < 0 || index >= _waveElements.Count)
            {
                Debug.LogError($"Invalid element index {index} for wave '{_title}'");
                return;
            }

            _waveElements.RemoveAt(index);
            EditorUtility.SetDirty(this);
        }

        public void ClearWaveElements()
        {
            _waveElements.Clear();
            EditorUtility.SetDirty(this);
        }

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(_title))
            {
                _title = "New Wave";
            }

            if (_waveElements == null)
            {
                _waveElements = new List<AbstractWaveElement>();
            }

            _waveElements.RemoveAll(element => element == null);
        }

        public void StartWave()
        {
            Debug.Log($"Starting wave: {_title}");
            CoroutineRunner.Start(RunWave());
        }

        private IEnumerator RunWave()
        {
            foreach (var element in _waveElements)
            {
                yield return element.Run();
            }
        }
    }
}