using System;
using System.Collections;

using UnityEngine;

namespace LevelSystem
{
    [CreateAssetMenu(fileName = "WaveDelay", menuName = "Game/Wave/Delay")]
    [Serializable]
    public class WaveDelay : AbstractWaveElement
    {
        [SerializeField]
        [Range(0f, 10f)]
        [Tooltip("Delay in seconds")]
        private float _delayTime = 1f;
        public float DelayTime => _delayTime;

        protected override IEnumerator Coroutine(Action onComplete)
        {
            yield return new WaitForSeconds(_delayTime);
            onComplete?.Invoke();
        }
    }
}