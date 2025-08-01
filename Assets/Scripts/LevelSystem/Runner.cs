using System;
using System.Collections;

namespace LevelSystem
{
    [Serializable]
    public abstract class Runner : TimeElement
    {
        public abstract IEnumerator Run();
    }
}