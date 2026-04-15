using UnityEngine;
using UnityEngine.UI;


public class BuyUpgrade : MonoBehaviour
{
    [SerializeField] private UpgradeAsset m_Asset;
    [SerializeField] private Image m_UpgradeImage;
    [SerializeField] private Text m_UpgradeText;
    [SerializeField] private Text m_LevelText, m_CostText;
    [SerializeField] private Button m_BuyButton;
    
    private int m_CostNumber = 0;

    private void Start()
    {
        if (m_Asset == null) return;

        m_UpgradeImage.sprite = m_Asset.m_Sprite;
        m_UpgradeText.text = m_Asset.m_UpgradeName;
    }

    public void Initialize()
    {
        var savedlevel = Upgrades.GetUpgradeLevel(m_Asset);

        if (savedlevel >= m_Asset.m_CostByLevel.Length)
        {
            m_LevelText.text = $"Lvl: MAX";
            m_BuyButton.interactable = false;
            m_CostText.text = "X";
            m_CostNumber = int.MaxValue;
        }
        else
        {
            m_LevelText.text = $"Lvl: {savedlevel + 1}";
            m_CostNumber = m_Asset.m_CostByLevel[savedlevel];
            m_CostText.text = m_CostNumber.ToString();
        }
    }

    public void Buy()
    {
        Upgrades.BuyUpgrade(m_Asset);
        Initialize();
    }

    public void CheckCost(int starsMoney)
    {
        print ($"Checking cost for {m_Asset.name}: have stars: {starsMoney}, need stars: {m_CostNumber}");  
        m_BuyButton.interactable = starsMoney >= m_CostNumber;
    }
}
