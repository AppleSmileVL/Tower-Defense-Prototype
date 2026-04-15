using UnityEngine;
using UnityEngine.UI;

public class UIUpgradeShop : MonoBehaviour
{
    [SerializeField] private int m_StarsMoney;
    [SerializeField] private Text m_StarsMoneyText;
    [SerializeField] private BuyUpgrade[] m_Sales;

    private void Start()
    {
        foreach (var slot in m_Sales)
        {
            slot.Initialize();
            var btn = slot.transform.Find("BuyButton").GetComponent<Button>();
            btn.onClick.AddListener(slot.Buy);
            btn.onClick.AddListener(UpdateStarsMoney);
        }
        UpdateStarsMoney();
    }

    public void UpdateStarsMoney()
    {
        print("Updating stars money...");
        m_StarsMoney = MapCompletion.Instance.TotalScore;
        m_StarsMoney -= Upgrades.GetTotalCost();
        m_StarsMoneyText.text = m_StarsMoney.ToString();
        foreach (var slot in m_Sales)
        {
            slot.Initialize();
            slot.CheckCost(m_StarsMoney);
        }
    }
}