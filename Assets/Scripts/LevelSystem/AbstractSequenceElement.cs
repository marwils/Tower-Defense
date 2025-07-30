using System;
using System.Collections;

using UnityEngine;

namespace LevelSystem
{
    [Serializable]
    public abstract class AbstractSequenceElement : ScriptableObject, ISequenceElement
    {
        public float Duration => GetDuration();

        public bool IsRunning => GetIsRunning();

        public abstract IEnumerator Run();

        protected abstract float GetDuration();

        protected abstract bool GetIsRunning();
    }
}