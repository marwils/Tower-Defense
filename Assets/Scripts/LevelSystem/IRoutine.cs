using System.Collections;

namespace LevelSystem
{
    public interface IRoutine
    {
        float Duration { get; }
        bool IsRunning { get; }

        IEnumerator Run();
    }
}