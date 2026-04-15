using UnityEngine;
using UnityEngine.UI;

public class UITowerBuyControl : MonoBehaviour
{
    [SerializeField] private TowerAsset m_TowerAsset;
    [SerializeField] private Text m_GoldCostText;
    [SerializeField] private Button m_ButtonTower;
    [SerializeField] private Transform m_BuildSite;

    public void SetTowerAsset(TowerAsset towerAsset) { m_TowerAsset = towerAsset; }
    public Transform SetBuildSite {  set { m_BuildSite = value; } }

    private void Start()
    {
        TDPlayer.SubscribeToGoldUpdate(GoldStatusCheck);

        if (m_TowerAsset == null)
        {
            if (m_ButtonTower != null) m_ButtonTower.interactable = false;
            enabled = false;
            return;
        }
        
        if (m_GoldCostText != null)
            m_GoldCostText.text = m_TowerAsset.m_GoldCost.ToString();
        
        if (m_ButtonTower != null)
        {
            var image = m_ButtonTower.GetComponent<Image>();

            if (image != null)
                image.sprite = m_TowerAsset.m_GUITowerSprite;
        }
    }

    private void GoldStatusCheck(int gold)
    {
        if (m_TowerAsset == null || m_ButtonTower == null || m_GoldCostText == null)
            return;

        if (gold >= m_TowerAsset.m_GoldCost != m_ButtonTower.interactable)
        {
            m_ButtonTower.interactable = !m_ButtonTower.interactable;
            m_GoldCostText.color = m_ButtonTower.interactable ? Color.white : Color.red;
        }
    }

    public void Buy()
    {
        TDPlayer.Instance.TryBuild(m_TowerAsset, m_BuildSite);
        m_BuildSite.gameObject.SetActive(false);
        UIBuildSite.HideControls();
        
    }
}
