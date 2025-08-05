using UnityEngine;
using UnityEngine.InputSystem;

[ExecuteInEditMode]
public class ColorMapToggle : MonoBehaviour
{
    [SerializeField]
    private Material _material;

    [SerializeField]
    private Texture2D _defaultTexture;

    [SerializeField]
    private Texture2D _alternateTexture;

    [SerializeField]
    private bool _showDefault = true;

    [SerializeField]
    [Tooltip("JUST FOR TESTING: Enable this to use the hotkey SCROLL LOCK in play mode.")]
    private bool _useHotKeyInPlayMode = false;

    void Update()
    {
        if (_useHotKeyInPlayMode && Keyboard.current.scrollLockKey.wasPressedThisFrame)
        {
            ToggleColorMap();
        }
    }

    private void OnValidate()
    {
        if (_material == null && GetComponent<Renderer>() != null)
        {
            _material = GetComponent<Renderer>().sharedMaterial;
        }

        if (_defaultTexture != null)
        {
            _material.SetTexture("_BaseMap", _defaultTexture);
        }

        UpdateTextures();
    }

    public void ToggleColorMap()
    {
        _showDefault = !_showDefault;
        UpdateTextures();
    }

    private void UpdateTextures()
    {
        if (_material == null || _defaultTexture == null || _alternateTexture == null)
        {
            return;
        }

        if (_showDefault)
            _material.SetTexture("_BaseMap", _defaultTexture);
        else
            _material.SetTexture("_BaseMap", _alternateTexture);
    }
}
