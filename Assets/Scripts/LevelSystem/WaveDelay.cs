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

        private bool _isRunning;

        public override IEnumerator Run()
        {
            _isRunning = true;
            Debug.Log($"Global delay for {_delayTime} seconds in {name}");
            yield return new WaitForSeconds(_delayTime);
            _isRunning = false;
        }

        protected override float GetDuration()
        {
            return _delayTime;
        }

        protected override bool GetIsRunning()
        {
            return _isRunning;
        }
    }
}