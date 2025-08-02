using UnityEngine;

namespace MarwilsTD.LevelSystem
{
    [System.Serializable]
    public abstract class TimeElement : ScriptableObject
    {
        public float Duration => GetDuration();
        protected abstract float GetDuration();

        public bool IsRunning => GetIsRunning();

        protected abstract bool GetIsRunning();
    }
}