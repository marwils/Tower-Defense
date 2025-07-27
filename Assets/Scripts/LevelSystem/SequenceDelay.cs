

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
        [Range(0f, 10f)]
        [SerializeField]
        private float _delayTime = 1f;

        protected override IEnumerator Coroutine()
        {
            yield return new WaitForSeconds(_delayTime);
        }
    }
}