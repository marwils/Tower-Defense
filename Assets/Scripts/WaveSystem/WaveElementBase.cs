using UnityEngine;
using UnityEngine.Events;

public abstract class WaveElementBase : ScriptableObject, IWaveElement
{
    private UnityEvent _onComplete = new UnityEvent();

    public UnityEvent OnComplete => _onComplete;

    public abstract void StartElement();
}