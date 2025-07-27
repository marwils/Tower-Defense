using System.Collections;

using Helper;

using UnityEngine;

[CreateAssetMenu(menuName = "WaveSystem/WaveDelay")]
public class WaveDelaySO : WaveElementBase
{
    [SerializeField]
    private float _delayTime = 1f;

    public override void StartElement()
    {
        CoroutineRunner.Start(DelayCoroutine());
    }

    private IEnumerator DelayCoroutine()
    {
        yield return new WaitForSeconds(_delayTime);
        OnComplete?.Invoke();
    }
}