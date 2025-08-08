using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(MyTowerController))]
public class MyTowerUI : MonoBehaviour
{
    private MyTowerController _towerController;

    private VisualElement _towerPanel;
    private VisualElement _upgradesPanel;
    private VisualElement _extensionsPanel;

    void Awake()
    {
        _towerController = GetComponent<MyTowerController>();

        if (_towerController == null)
        {
            Debug.LogWarning("MyTowerController component is not found on the GameObject.");
            Destroy(this);
            return;
        }

        UIElementRegister.OnUIInitialized += OnUIInitialized;
        UIElementRegister.OnShowUpgradesRequested += ShowUpgrades;
        UIElementRegister.OnShowExtensionsRequested += ShowExtensions;

        _towerController.OnSelected += ShowTowerPanel;
        InputManager.TryRegister((input) => input.OnDeselect += HideAllPanels);
    }

    private void ShowTowerPanel()
    {
        ShowPanel(_towerPanel);
    }

    private void OnUIInitialized(VisualElement element)
    {
        _towerPanel = element.Q<VisualElement>("TowerPnl");
        _upgradesPanel = element.Q<VisualElement>("UpgradesPnl");
        _extensionsPanel = element.Q<VisualElement>("ExtensionsPnl");

        HideAllPanels();

        // for safety, ensure panels are displayed correctly
        SetDisplayForPanels(new List<VisualElement> { _towerPanel, _upgradesPanel, _extensionsPanel });
    }

    private void SetDisplayForPanels(List<VisualElement> visualElements, DisplayStyle displayStyle = DisplayStyle.Flex)
    {
        foreach (var element in visualElements)
        {
            element.style.display = displayStyle;
        }
    }

    private void ShowUpgrades()
    {
        HideAllPanels();
        _upgradesPanel.Clear();
        CreateUpgradesButtons(_towerController.MainNode);
        CreateBackButton(_upgradesPanel);
        ShowPanel(_upgradesPanel);
    }

    private void ShowExtensions()
    {
        HideAllPanels();
        _extensionsPanel.Clear();
        CreateExtensionsButtons(_towerController.MainNode);
        CreateBackButton(_extensionsPanel);
        ShowPanel(_extensionsPanel);
    }

    private void CreateUpgradesButtons(MyTowerNode node)
    {
        if (node == null)
            return;

        MyTowerNode latestUpgrade = (MyTowerNode)node.LatestUpgradeNode;

        if (
            latestUpgrade == null
            || latestUpgrade.AvailableUpgrades == null
            || latestUpgrade.AvailableUpgrades.Length == 0
        )
            return;

        UIElementRegister.Instance.CreateLabel(
            $"{latestUpgrade.Name} Upgrades",
            $"{GetSafeElementName(latestUpgrade.Name)}_UpgradesLbl",
            _upgradesPanel
        );

        foreach (MyTowerNode upgradeNode in latestUpgrade.AvailableUpgrades)
        {
            UIElementRegister.Instance.CreateButton(
                upgradeNode.Name,
                GetSafeElementName(upgradeNode.Name) + "Btn",
                _upgradesPanel,
                () =>
                {
                    latestUpgrade.UpgradeTo(upgradeNode);
                    HidePanel(_upgradesPanel);
                    ShowPanel(_towerPanel);
                }
            );
        }
    }

    private void CreateExtensionsButtons(MyTowerNode node)
    {
        MyTowerNode latestUpgrade = (MyTowerNode)node.LatestUpgradeNode;

        if (
            latestUpgrade == null
            || latestUpgrade.AvailableExtensions == null
            || latestUpgrade.AvailableExtensions.Length == 0
        )
            return;

        UIElementRegister.Instance.CreateLabel(
            $"{latestUpgrade.Name} Extensions",
            $"{GetSafeElementName(latestUpgrade.Name)}_ExtensionsLbl",
            _extensionsPanel
        );

        foreach (MyTowerNode extensionNode in latestUpgrade.AvailableExtensions)
        {
            UIElementRegister.Instance.CreateButton(
                extensionNode.Name,
                GetSafeElementName(extensionNode.Name) + "Btn",
                _extensionsPanel,
                () =>
                {
                    latestUpgrade.ExtendWith(extensionNode);
                    HidePanel(_extensionsPanel);
                    ShowPanel(_towerPanel);
                }
            );
        }
    }

    private void CreateBackButton(VisualElement parent)
    {
        UIElementRegister.Instance.CreateButton(
            "Back",
            "BackBtn",
            parent,
            () =>
            {
                HidePanel(_upgradesPanel);
                HidePanel(_extensionsPanel);
                ShowPanel(_towerPanel);
            }
        );
    }

    private static string GetSafeElementName(string baseName)
    {
        var safe = Regex.Replace(baseName, @"[^a-zA-Z0-9_]", "_");
        return safe + "Btn";
    }

    private void ShowPanel(VisualElement panel)
    {
        Debug.Log($"Showing panel: {panel.name}");
        panel.style.opacity = 1f;
        panel.pickingMode = PickingMode.Position;
        panel.style.translate = new Translate(0, 0);
    }

    private void HideAllPanels()
    {
        HidePanel(_towerPanel);
        HidePanel(_upgradesPanel);
        HidePanel(_extensionsPanel);
    }

    private void HidePanel(VisualElement panel)
    {
        panel.style.opacity = 0f;
        panel.pickingMode = PickingMode.Ignore;
        panel.style.translate = new Translate(Length.Percent(100), 0);
    }
}
