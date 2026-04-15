using System;
using UnityEngine;

public class Upgrades : SingletonBase<Upgrades>
{
    public const string filename = "upgrades.dat";

    [Serializable]
    private class UpgradeSave
    {
        public UpgradeAsset m_Asset;
        public int m_Level = 0;
    }

    [SerializeField] private UpgradeSave[] save;

    public static event Action OnUpgradeApplied;

    private void Awake()
    {
        Init();
        Saver<UpgradeSave[]>.TryLoad(filename, ref save);

        if (save == null)
            save = Array.Empty<UpgradeSave>();
    }

    public static void BuyUpgrade(UpgradeAsset asset)
    {
        if (Instance == null || Instance.save == null)
            return;

        foreach (var upgrade in Instance.save)
        {
            if (upgrade?.m_Asset == asset)
            {
                upgrade.m_Level++;
                Saver<UpgradeSave[]>.Save(filename, Instance.save);
                OnUpgradeApplied?.Invoke();
                return;
            }
        }
    }

    public static int GetTotalCost() 
    {
        int totalCost = 0;
        foreach (var upgrade in Instance.save)
        {
            if (upgrade != null && upgrade.m_Level > 0)
            {
                for (int i = 0; i < upgrade.m_Level; i++)
                {
                    if (i < upgrade.m_Asset.m_CostByLevel.Length) 
                    {
                        totalCost += upgrade.m_Asset.m_CostByLevel[i]; 
                    }
                }
            }
        }
        return totalCost;
    }

    public static int GetUpgradeLevel(UpgradeAsset asset)
    {
        if (Instance == null || Instance.save == null)
            return 0;

        foreach (var upgrade in Instance.save)
        {
            if (upgrade?.m_Asset == asset)
            {
                return upgrade.m_Level;
            }
        }
        return 0;
    }

    public static void ClearData()
    {
        if (Instance != null)
        {
            Instance.save = new UpgradeSave[0];
            Saver<UpgradeSave[]>.Save(filename, Instance.save);
        }
    }
}
