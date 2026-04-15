using UnityEngine.EventSystems;

public class UINullBuildSite : UIBuildSite
{
    public override void OnPointerDown(PointerEventData eventData)
    {
        HideControls();
    }
}
