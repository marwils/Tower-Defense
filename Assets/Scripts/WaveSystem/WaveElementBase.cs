using UnityEngine;
using UnityEngine.Events;

public abstract class WaveElementBase : ScriptableObject, IWaveElement
{
    [HideInInspector]
    public UnityEvent onComplete = new UnityEvent();

    public UnityEvent OnComplete => onComplete;

    public abstract void StartElement();
}