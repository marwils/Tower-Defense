using MarwilsTD;
using UnityEngine;

public class MyTowerNode : TowerNode
{
    [Header("My Tower Node")]
    [SerializeField]
    [Tooltip("Defines on which height the next tower node will be placed.")]
    private float _height = 1.0f;
    public float TotalHeight
    {
        get { return GetTotalHeight(); }
    }

    [SerializeField]
    [Tooltip("Defines the offset on the Y-axis for the tower node's position (upon another tower node).")]
    private float _yOffset;
    public float YOffset => _yOffset;

    [SerializeField]
    [Tooltip(
        "If true, the current tower node will be set to inactive when upgraded. This is useful for root nodes that should not be visible after an upgrade."
    )]
    private bool _replaceCurrentNodeOnUpgrade = false;
    public bool ReplaceCurrentNodeOnUpgrade => _replaceCurrentNodeOnUpgrade;

    public void SetY(float y)
    {
        transform.position = new Vector3(transform.position.x, y + _yOffset, transform.position.z);
    }

    public bool UpgradeTo(MyTowerNode upgradeNode)
    {
        if (HasUpgrade)
        {
            var currentUpgrade = GetUpgradeNode();
            if (currentUpgrade is MyTowerNode myCurrentUpgrade)
            {
                if (myCurrentUpgrade.ReplaceCurrentNodeOnUpgrade)
                {
                    gameObject.SetActive(true);
                }
                else
                {
                    upgradeNode.SetY(transform.position.y + _height + _yOffset);
                }
            }

            currentUpgrade.gameObject.SetActive(false);
        }

        SetUpgradeNode(upgradeNode);

        if (HasUpgrade)
        {
            upgradeNode.gameObject.SetActive(true);

            if (upgradeNode.ReplaceCurrentNodeOnUpgrade)
            {
                gameObject.SetActive(false);
            }
            else
            {
                upgradeNode.SetY(transform.position.y + _height + _yOffset);
            }
        }

        return HasUpgrade;
    }

    public bool ExtendWith(MyTowerNode extensionNode)
    {
        GetExtensionNode()?.gameObject.SetActive(false);

        SetExtensionNode(extensionNode);

        if (HasExtension)
        {
            extensionNode.gameObject.SetActive(true);
            extensionNode.SetY(transform.position.y + _height + _yOffset);
        }

        return HasExtension;
    }

    private float GetTotalHeight()
    {
        return GetTotalHeightRecursively(this);
    }

    private float GetTotalHeightRecursively(MyTowerNode myTowerNode)
    {
        float totalHeight = myTowerNode._height;

        if (myTowerNode.HasExtension)
        {
            totalHeight += GetTotalHeightRecursively(myTowerNode.CurrentExtension as MyTowerNode);
        }

        return totalHeight;
    }

    private void OnDrawGizmosSelected()
    {
        if (_yOffset != 0)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.orange;
        }

        var height = _yOffset + GetTotalHeight();
        Gizmos.DrawWireCube(
            transform.position + Vector3.up * _yOffset + Vector3.up * height / 2,
            new Vector3(1, height, 1)
        );
    }
}
