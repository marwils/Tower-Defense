using System;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class UIButtonRegister : MonoBehaviour
{
    private static UIButtonRegister _instance;

    public static UIButtonRegister Instance => _instance;
    public static event Action<VisualElement> OnUIInitialized;
    public static event Action OnShowUpgradesRequested;
    public static event Action OnShowExtensionsRequested;

    private UIDocument _uiDocument;

    private VisualElement _root;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Debug.LogWarning("Multiple instances of UIButtonRegister detected. Destroying the new instance.");
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    void OnEnable()
    {
        _uiDocument = GetComponent<UIDocument>();
        _root = _uiDocument.rootVisualElement;

        OnUIInitialized?.Invoke(_root);

        // Register button callbacks
        var upgradeButton = _root.Q<Button>("ShowUpgradesBtn");
        upgradeButton.RegisterCallback<ClickEvent>(ev => OnShowUpgradesRequested?.Invoke());

        var extendButton = _root.Q<Button>("ShowExtensionsBtn");
        extendButton.RegisterCallback<ClickEvent>(ev => OnShowExtensionsRequested?.Invoke());
    }

    public void CreateButton(string text, string name, VisualElement parent, Action onClick)
    {
        var button = new Button(() => onClick?.Invoke()) { text = text, name = name };
        parent.Add(button);
    }

    public void CreateLabel(string text, string name, VisualElement parent)
    {
        var label = new Label(text) { name = name };
        parent.Add(label);
    }
}
