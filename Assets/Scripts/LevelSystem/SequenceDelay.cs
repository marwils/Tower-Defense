

namespace LevelSystem
{
    using System;
    using System.Collections;

    using UnityEngine;

    [CreateAssetMenu(fileName = "SequenceDelay", menuName = "Game/Sequence/Delay")]
    [Serializable]
    public class SequenceDelay : AbstractSequenceElement
    {
        [Tooltip("Delay in seconds")]
        [Range(0f, 5f)]
        [SerializeField]
        private float _delayTime = 1f;

        protected override IEnumerator Coroutine(Vector3 spawnPoint, Action onComplete)
        {
            yield return new WaitForSeconds(_delayTime);
            onComplete?.Invoke();
        }
    }
}