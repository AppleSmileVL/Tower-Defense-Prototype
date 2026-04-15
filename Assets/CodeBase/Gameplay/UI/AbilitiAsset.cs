using UnityEngine;

[CreateAssetMenu]
public class AbilitiAsset : ScriptableObject
{
    [Header("Cost")]
    public int m_ManaCost = 50;

    [Header("Visual")]
    public Sprite m_AbilityIcon;

    [Header("Upgrades")]
    [SerializeField] private UpgradeAsset m_RequiredUpgrade;
    [SerializeField] private int m_RequiredUpgradeLevel;
    public bool IsAvailable() => !m_RequiredUpgrade || m_RequiredUpgradeLevel <= Upgrades.GetUpgradeLevel(m_RequiredUpgrade);


}
