using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "WaveSystem/WaveDelay")]
public class WaveDelaySO : WaveElementBase
{
    public float delayTime = 1f;

    public override void StartElement()
    {
        CoroutineRunner.Instance.StartCoroutine(DelayCoroutine());
    }

    private IEnumerator DelayCoroutine()
    {
        yield return new WaitForSeconds(delayTime);
        onComplete?.Invoke();
    }
}