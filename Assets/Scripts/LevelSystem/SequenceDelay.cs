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

        private bool _isRunning;

        public override IEnumerator Run()
        {
            _isRunning = true;
            Debug.Log($"Sequence delay for {_delayTime} seconds in {name}");
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