using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIBuildSite : MonoBehaviour, IPointerDownHandler
{
    public TowerAsset[] buildableTowers;
    public void SetBuildableTower(TowerAsset[] towers) 
    {
        if (towers == null || towers.Length == 0)
        {
            Destroy(transform.parent.gameObject);
        }
        else
        {
            buildableTowers = towers;
        }
    }
    
    public static event Action<UIBuildSite> OnClickEvent;

    public static void HideControls()
    {
        OnClickEvent?.Invoke(null);
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        OnClickEvent?.Invoke(this);
    }
}
