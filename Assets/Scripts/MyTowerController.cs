using System;

using MarwilsTD;

using UnityEngine;

public class MyTowerController : TowerController
{
    [Header("My Tower Controller")]

    [SerializeField]
    [Tooltip("Main tower node. This is the root node of the tower.")]
    private MyTowerNode _mainNode;

    protected override void Awake()
    {
        base.Awake();

        if (_mainNode == null)
        {
            Debug.LogWarning($"Root node is not set in tower <{gameObject.name}>.");
            Destroy(gameObject);
            return;
        }

        DeactivateUpgradesRecursively(_mainNode);
        DeactivateExtensionsRecursively(_mainNode);
    }

    public void ExtendTower()
    {
        throw new NotImplementedException();
    }

    public void UpgradeTower(MyTowerNode upgradeNode)
    {
        if (upgradeNode == null)
        {
            Debug.LogWarning($"Upgrade node is null in tower <{gameObject.name}>.");
            return;
        }

        _mainNode.CurrentUpgrade = upgradeNode;
    }

    private void DeactivateUpgradesRecursively(MyTowerNode node)
    {
        foreach (MyTowerNode upgradeNode in node.AvailableUpgrades)
        {
            if (upgradeNode != null)
            {
                DeactivateUpgradesRecursively(upgradeNode);
                upgradeNode.gameObject.SetActive(false);
            }
        }
    }

    private void DeactivateExtensionsRecursively(MyTowerNode node)
    {
        foreach (MyTowerNode extensionNode in node.AvailableExtensions)
        {
            if (extensionNode != null)
            {
                DeactivateExtensionsRecursively(extensionNode);
                extensionNode.gameObject.SetActive(false);
            }
        }
    }
}
