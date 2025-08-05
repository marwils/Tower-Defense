using System;
using System.Collections;

namespace MarwilsTD.LevelSystem
{
    [Serializable]
    public abstract class Runner : TimeElement
    {
        public abstract IEnumerator Run();
    }
}
