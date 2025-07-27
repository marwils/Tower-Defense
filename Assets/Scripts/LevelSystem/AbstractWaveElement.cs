using System;
using System.Collections;

using Helper;

using UnityEngine;
using UnityEngine.Events;

namespace LevelSystem
{
    [Serializable]
    public abstract class AbstractWaveElement : ScriptableObject, IWaveElement
    {
        [SerializeField, HideInInspector]
        private UnityEvent _onComplete = new();

        // Public property to satisfy interface
        public UnityEvent OnComplete => _onComplete;

        [System.NonSerialized]
        private Coroutine _coroutine;

        public void StartElement()
        {
            _coroutine = CoroutineRunner.Start(Coroutine(Complete));
        }

        protected abstract IEnumerator Coroutine(Action onComplete);

        private void Complete()
        {
            _onComplete?.Invoke();
            if (_coroutine != null)
            {
                CoroutineRunner.Stop(_coroutine);
                _coroutine = null;
            }
        }
    }
}