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

        UIButtonRegister.OnUIInitialized += OnUIInitialized;
        UIButtonRegister.OnShowUpgradesRequested += ShowUpgrades;
        UIButtonRegister.OnShowExtensionsRequested += ShowExtensions;
    }

    private void OnUIInitialized(VisualElement element)
    {
        _towerPanel = element.Q<VisualElement>("TowerPnl");
        _upgradesPanel = element.Q<VisualElement>("UpgradesPnl");
        _extensionsPanel = element.Q<VisualElement>("ExtensionsPnl");

        HidePanel(_upgradesPanel);
        HidePanel(_extensionsPanel);

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
        HidePanel(_towerPanel);
        _upgradesPanel.Clear();
        CreateUpgradesButtons(_towerController.MainNode);
        CreateBackButton(_upgradesPanel);
        ShowPanel(_upgradesPanel);
    }

    private void ShowExtensions()
    {
        HidePanel(_towerPanel);
        _extensionsPanel.Clear();
        CreateExtensionsButtons(_towerController.MainNode);
        CreateBackButton(_extensionsPanel);
        ShowPanel(_extensionsPanel);
    }

    private void CreateUpgradesButtons(MyTowerNode node)
    {
        if (node == null || node.AvailableUpgrades == null || node.AvailableUpgrades.Length == 0)
            return;

        if (node.HasUpgrade)
        {
            CreateUpgradesButtons((MyTowerNode)node.CurrentUpgrade);
        }

        UIButtonRegister.Instance.CreateLabel(
            $"{node.Name} Upgrades",
            $"{GetSafeElementName(node.Name)}_UpgradesLbl",
            _upgradesPanel
        );

        foreach (MyTowerNode upgradeNode in node.AvailableUpgrades)
        {
            UIButtonRegister.Instance.CreateButton(
                upgradeNode.Name,
                GetSafeElementName(upgradeNode.Name) + "Btn",
                _upgradesPanel,
                () =>
                {
                    node.UpgradeTo(upgradeNode);
                }
            );
        }
    }

    private void CreateExtensionsButtons(MyTowerNode node)
    {
        if (node == null || node.AvailableExtensions == null || node.AvailableExtensions.Length == 0)
            return;

        if (node.HasExtension)
        {
            CreateExtensionsButtons((MyTowerNode)node.CurrentExtension);
        }

        UIButtonRegister.Instance.CreateLabel(
            $"{node.Name} Extensions",
            $"{GetSafeElementName(node.Name)}_ExtensionsLbl",
            _extensionsPanel
        );

        foreach (MyTowerNode extensionNode in node.AvailableExtensions)
        {
            UIButtonRegister.Instance.CreateButton(
                extensionNode.Name,
                GetSafeElementName(extensionNode.Name) + "Btn",
                _extensionsPanel,
                () =>
                {
                    node.ExtendWith(extensionNode);
                }
            );
        }
    }

    private void CreateBackButton(VisualElement parent)
    {
        UIButtonRegister.Instance.CreateButton(
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
        panel.style.opacity = 1f;
        panel.pickingMode = PickingMode.Position;
        panel.style.translate = new Translate(0, 0);
    }

    private void HidePanel(VisualElement panel)
    {
        panel.style.opacity = 0f;
        panel.pickingMode = PickingMode.Ignore;
        panel.style.translate = new Translate(Length.Percent(100), 0);
    }
}
