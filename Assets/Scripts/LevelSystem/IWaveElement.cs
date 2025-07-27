using UnityEngine.Events;

namespace LevelSystem
{
    public interface IWaveElement
    {
        void StartElement();

        UnityEvent OnComplete { get; }
    }
}