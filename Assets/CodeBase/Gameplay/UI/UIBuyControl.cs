using System.Collections.Generic;
using UnityEngine;

public class UIBuyControl : MonoBehaviour
{
    [SerializeField] private UITowerBuyControl m_TowerBuyPrefab;

    private List<UITowerBuyControl> m_ActiveControl;
    private RectTransform m_RectTransform;
    
    private Canvas m_ParentCanvas;
    private Camera m_UICamera;

    private void Awake()
    {
        
        m_RectTransform = GetComponent<RectTransform>();
        m_ParentCanvas = GetComponentInParent<Canvas>();

        if (m_ParentCanvas != null && m_ParentCanvas.renderMode != RenderMode.ScreenSpaceOverlay)
            m_UICamera = m_ParentCanvas.worldCamera;
        else
            m_UICamera = null;
        
        m_RectTransform = GetComponent<RectTransform>();
        UIBuildSite.OnClickEvent += MoveToTransform;
        gameObject.SetActive(false);
        m_ActiveControl = new List<UITowerBuyControl>();
    }

    private void OnDestroy()
    {
        UIBuildSite.OnClickEvent -= MoveToTransform;
    }

    private void MoveToTransform(UIBuildSite m_BuildSite)
    {
        if (m_BuildSite == null)
        {
            foreach (var control in m_ActiveControl) Destroy(control.gameObject);
            m_ActiveControl.Clear();
            gameObject.SetActive(false);
            return;
        }

        // Экранная точка клика по world-позиции цели
        Vector2 screenPos = Camera.main.WorldToScreenPoint(m_BuildSite.transform.position);

        // Конвертация экранной точки в локальную координату RectTransform
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)m_RectTransform.parent,
            screenPos,
            m_UICamera,
            out Vector2 localPos
        );

        m_RectTransform.anchoredPosition = localPos;

        gameObject.SetActive(true);

        // Создаем иконки
        m_ActiveControl.Clear();
        foreach (var asset in m_BuildSite.buildableTowers)
        {
            if (asset.IsAvailable())
            {
                var newControl = Instantiate(m_TowerBuyPrefab, transform);
                m_ActiveControl.Add(newControl);
                newControl.SetTowerAsset(asset);
            }
        }

        // Размещаем иконки по кругу BuildSite
        if (m_ActiveControl.Count > 0)
        {
            gameObject.SetActive(true);
            float angle = 360f / m_ActiveControl.Count;
            for (int i = 0; i < m_ActiveControl.Count; i++)
            {
                var offset = Quaternion.AngleAxis(angle * i, Vector3.forward) * Vector3.up * 110f;
                m_ActiveControl[i].GetComponent<RectTransform>().anchoredPosition = offset;
            }
        }

        foreach (var tbc in GetComponentsInChildren<UITowerBuyControl>())
        {
            tbc.SetBuildSite = m_BuildSite.transform.parent.parent;
        }  
    }


}
