using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIClickProtection : SingletonBase<UIClickProtection>, IPointerClickHandler
{
    private Image blocker;
    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        blocker = GetComponent<Image>();
    }
    private Action<Vector2> m_OnClickAction;

    public void Activate(Action<Vector2> mouseAction)
    {
        blocker.enabled = true;
        m_OnClickAction = mouseAction;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Вызываем действие пока блокер ещё активен, чтобы тот же клик не прошёл на UI под блокером.
        var pos = eventData.pressPosition;
        m_OnClickAction?.Invoke(pos);
        m_OnClickAction = null;
        StartCoroutine(DisableBlockerNextFrame());
    }

    private IEnumerator DisableBlockerNextFrame()
    {
        yield return null;
        if (blocker != null)
            blocker.enabled = false;
    }
}
