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

        public override IEnumerator Run()
        {
            Debug.Log($"Sequence delay for {_delayTime} seconds in {name}");
            yield return new WaitForSeconds(_delayTime);
        }
    }
}