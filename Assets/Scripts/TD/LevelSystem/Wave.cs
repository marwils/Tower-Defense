using System.Collections;
using System.Collections.Generic;

using Helper;

using UnityEditor;

using UnityEngine;

namespace LevelSystem
{
    public class Wave : TimeElement
    {
        [SerializeField]
        private string _title = "New Wave";
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        [SerializeField]
        private List<WaveElement> _waveElements = new();
        public IReadOnlyList<WaveElement> WaveElements => _waveElements;

        public void AddWaveElement(WaveElement element)
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
                _waveElements = new List<WaveElement>();
            }

            _waveElements.RemoveAll(element => element == null);
        }

        public void StartWave()
        {
            CoroutineRunner.Start(RunWave());
        }

        private IEnumerator RunWave()
        {
            foreach (var element in _waveElements)
            {
                yield return element.Run();
            }
        }

        protected override float GetDuration()
        {
            float totalDuration = 0f;
            foreach (var element in _waveElements)
            {
                totalDuration += element.Duration;
            }
            return totalDuration;
        }

        protected override bool GetIsRunning()
        {
            foreach (var element in _waveElements)
            {
                if (element.IsRunning)
                {
                    return true;
                }
            }
            return false;
        }
    }
}