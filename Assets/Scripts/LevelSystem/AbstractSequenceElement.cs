using System.Collections;

using UnityEngine;

namespace LevelSystem
{
    [System.Serializable]
    public abstract class AbstractSequenceElement : ScriptableObject, ISequenceElement
    {
        public float Duration => throw new System.NotImplementedException();

        public bool IsRunning => throw new System.NotImplementedException();

        public abstract IEnumerator Run();
    }
}