using System;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class UIButtonRegister : MonoBehaviour
{
    public static event Action<VisualElement> OnUIInitialized;
    public static event Action OnShowUpgradesRequested;
    public static event Action OnShowExtensionsRequested;

    private UIDocument _uiDocument;
    private VisualElement _root;

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
}
