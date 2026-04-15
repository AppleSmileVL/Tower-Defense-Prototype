using UnityEngine;


[CreateAssetMenu]
public class TowerAsset : ScriptableObject
{
    [Header("Cost")]
    public int m_GoldCost = 15;

    [Header("Visual Model")]
    public Sprite m_TowerSprite;
    public Sprite m_GUITowerSprite;
    public Color color = Color.white;

    [Header("Weapon")]
    public TurretProperties m_TurretProperties;

    [Header("Upgrades")]
    [SerializeField] private UpgradeAsset m_RequiredUpgrade;
    [SerializeField] private int m_RequiredUpgradeLevel;
    public bool IsAvailable() => !m_RequiredUpgrade || m_RequiredUpgradeLevel <= Upgrades.GetUpgradeLevel(m_RequiredUpgrade);

    public TowerAsset[] m_UpgradeTo;
 }
