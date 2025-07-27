using UnityEngine.Events;

namespace LevelSystem
{
    public interface ISequenceElement
    {
        void StartElement();

        UnityEvent OnComplete { get; }
    }
}