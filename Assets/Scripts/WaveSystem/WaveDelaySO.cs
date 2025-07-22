using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "WaveSystem/WaveDelay")]
public class WaveDelaySO : WaveElementBase
{
    [SerializeField]
    private float _delayTime = 1f;

    public override void StartElement()
    {
        CoroutineRunner.Instance.StartCoroutine(DelayCoroutine());
    }

    private IEnumerator DelayCoroutine()
    {
        yield return new WaitForSeconds(_delayTime);
        onComplete?.Invoke();
    }
}