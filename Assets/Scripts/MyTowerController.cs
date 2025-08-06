using System.Linq;
using MarwilsTD;
using UnityEngine;

public class MyTowerController : TowerController
{
    [Header("My Tower Controller")]
    [SerializeField]
    [Tooltip("Main tower node. This is the root node of the tower.")]
    private MyTowerNode _mainNode;
    public MyTowerNode MainNode => _mainNode;

    protected override void Awake()
    {
        base.Awake();

        if (_mainNode == null)
        {
            Debug.LogWarning($"Root node is not set in tower <{gameObject.name}>.");
            Destroy(gameObject);
            return;
        }

        SetUpgradesAndExtensionsActiveRecursively(_mainNode, false);
    }

    public void UpgradeTo(MyTowerNode upgradeNode)
    {
        _mainNode.UpgradeTo(upgradeNode);
    }

    public void ExtendWith(MyTowerNode extensionNode)
    {
        _mainNode.ExtendWith(extensionNode);
    }

    private void SetUpgradesAndExtensionsActiveRecursively(TowerNode node, bool isActive = true)
    {
        if (node == null)
            return;

        TowerNode[] upgradesAndExtensions = node.AvailableUpgrades.Concat(node.AvailableExtensions).ToArray();

        foreach (var childNode in upgradesAndExtensions)
        {
            if (childNode != null)
            {
                childNode.gameObject.SetActive(isActive);
                SetUpgradesAndExtensionsActiveRecursively(childNode, isActive);
            }
        }
    }
}
