using System;
using System.Collections;

using UnityEngine;

namespace LevelSystem
{
    [Serializable]
    public abstract class AbstractWaveElement : ScriptableObject, IWaveElement
    {
        public float Duration => throw new NotImplementedException();

        public bool IsRunning => throw new NotImplementedException();

        public abstract IEnumerator Run();
    }
}