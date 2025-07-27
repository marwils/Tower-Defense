using System;
using System.Collections;

using Helper;

using UnityEngine;
using UnityEngine.Events;

namespace LevelSystem
{
    [Serializable]
    public abstract class AbstractSequenceElement : ScriptableObject, ISequenceElement
    {
        private UnityEvent _onComplete = new UnityEvent();

        public UnityEvent OnComplete => _onComplete;

        [System.NonSerialized]
        private Coroutine _coroutine;

        public void StartElement(Vector3 spawnPoint)
        {
            _coroutine = CoroutineRunner.Start(Coroutine(spawnPoint, () => Complete()));
        }

        protected abstract IEnumerator Coroutine(Vector3 spawnPoint, Action onComplete);

        private void Complete()
        {
            OnComplete?.Invoke();
            CoroutineRunner.Stop(_coroutine);
        }
    }
}