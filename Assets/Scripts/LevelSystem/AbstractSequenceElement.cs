using System.Collections;

using Helper;

using UnityEngine;
using UnityEngine.Events;

namespace LevelSystem
{
    [System.Serializable]
    public abstract class AbstractSequenceElement : ScriptableObject, ISequenceElement
    {
        private UnityEvent _onComplete = new UnityEvent();
        public UnityEvent OnComplete => _onComplete;

        [System.NonSerialized]
        private Coroutine _coroutine;

        public void StartElement()
        {
            _coroutine = CoroutineRunner.Start(ExecuteWithCompletion());
        }

        private IEnumerator ExecuteWithCompletion()
        {
            yield return Coroutine();
            Complete();
        }

        protected abstract IEnumerator Coroutine();

        private void Complete()
        {
            OnComplete?.Invoke();
            CoroutineRunner.Stop(_coroutine);
        }
    }
}