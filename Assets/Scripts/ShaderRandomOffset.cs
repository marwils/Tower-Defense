using UnityEngine;

public class ShaderRandomOffset : MonoBehaviour
{
    private const string PropertyName = "_RandomOffset";

    [SerializeField]
    private float _randomOffsetRange = 10f;

    private MaterialPropertyBlock _block;
    private Renderer _renderer;
    private static readonly int _randomOffsetId = Shader.PropertyToID(PropertyName);

    void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _block = new MaterialPropertyBlock();
    }

    void Start()
    {
        float randomOffset = Random.Range(0f, _randomOffsetRange);

        // Lese aktuelle Properties (wichtig!)
        _renderer.GetPropertyBlock(_block);

        // Setze neuen Wert
        _block.SetFloat(_randomOffsetId, randomOffset);

        // Wende PropertyBlock auf das Renderer-Material an
        _renderer.SetPropertyBlock(_block);
    }
}
