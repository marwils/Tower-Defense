using UnityEngine;

public class ShaderRandomOffset : MonoBehaviour
{
    private const string PropertyName = "_RandomOffset";

    [SerializeField]
    private float _randomOffsetRange = 10f;

    private MaterialPropertyBlock _block;
    private Renderer _renderer;
    private static readonly int _randomOffsetId = Shader.PropertyToID(PropertyName);

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _block = new MaterialPropertyBlock();
    }

    private void Start()
    {
        float randomOffset = Random.Range(0f, _randomOffsetRange);

        _renderer.GetPropertyBlock(_block);

        _block.SetFloat(_randomOffsetId, randomOffset);

        _renderer.SetPropertyBlock(_block);
    }
}
