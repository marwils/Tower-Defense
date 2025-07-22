using UnityEngine.Events;

public interface IWaveElement
{
    UnityEvent OnComplete { get; }

    void StartElement();
}