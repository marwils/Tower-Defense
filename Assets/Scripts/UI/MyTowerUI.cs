using UnityEngine;
using UnityEngine.UIElements;

public class MyTowerUI : MonoBehaviour
{
    private VisualElement _towerPanel;
    private VisualElement _upgradesPanel;
    private VisualElement _extensionsPanel;

    void Awake()
    {
        UIButtonRegister.OnUIInitialized += OnUIInitialized;
        UIButtonRegister.OnShowUpgradesRequested += ShowUpgrades;
        UIButtonRegister.OnShowExtensionsRequested += ShowExtensions;
    }

    private void OnUIInitialized(VisualElement element)
    {
        _towerPanel = element.Q<VisualElement>("TowerPnl");
        _upgradesPanel = element.Q<VisualElement>("UpgradesPnl");
        _extensionsPanel = element.Q<VisualElement>("ExtensionsPnl");

        // Verstecke Panels mit Opacity statt Display
        HidePanel(_upgradesPanel);
        HidePanel(_extensionsPanel);
    }

    private void ShowUpgrades()
    {
        HidePanel(_towerPanel);
        ShowPanel(_upgradesPanel);
    }

    private void ShowExtensions()
    {
        HidePanel(_towerPanel);
        ShowPanel(_extensionsPanel);
    }

    private void ShowPanel(VisualElement panel)
    {
        panel.style.opacity = 1f;
        panel.pickingMode = PickingMode.Position;
        panel.style.translate = new Translate(0, 0);
    }

    private void HidePanel(VisualElement panel)
    {
        panel.style.opacity = 0f;
        panel.pickingMode = PickingMode.Ignore;
        panel.style.translate = new Translate(Length.Percent(100), 0); // Nach rechts verschieben
    }
}
