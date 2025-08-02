using System.Collections;

namespace MarwilsTD.LevelSystem
{
    public interface IRoutine
    {
        float Duration { get; }
        bool IsRunning { get; }
        IEnumerator Run();
    }
}