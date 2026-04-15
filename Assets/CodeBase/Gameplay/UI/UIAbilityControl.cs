using UnityEngine;
using UnityEngine.UI;

public class UIAbilityControl : MonoBehaviour
{
    [SerializeField] private AbilitiAsset m_AbilityAsset;
    [SerializeField] private Text m_ManaCostText;
    [SerializeField] private Button m_ButtonAbility;
    [SerializeField] private AbilityType m_AbilityType;

    public enum AbilityType { Fire, Time }

    private void Start()
    {
        // Проверяем доступность и активируем/деактивируем UI
        if (m_AbilityAsset == null)
        {
            gameObject.SetActive(false);
            return;
        }

        // Если абилка недоступна - скрываем
        if (!m_AbilityAsset.IsAvailable())
        {
            gameObject.SetActive(false);
            return;
        }

        // Показываем UI если абилка доступна
        gameObject.SetActive(true);

        if (m_ManaCostText != null)
            m_ManaCostText.text = m_AbilityAsset.m_ManaCost.ToString();

        if (m_ButtonAbility != null)
        {
            var image = m_ButtonAbility.GetComponent<Image>();
            if (image != null && m_AbilityAsset.m_AbilityIcon != null)
                image.sprite = m_AbilityAsset.m_AbilityIcon;

            m_ButtonAbility.onClick.AddListener(TryUseAbility);
        }

        // Подписываемся на изменение маны
        TDPlayer.SubscribeToManaChanged(UpdateManaStatus);
        UpdateManaStatus(TDPlayer.Instance.Mana);

        // Подписываемся на апгрейды для активации абилок
        Upgrades.OnUpgradeApplied += CheckAvailability;
    }

    private void CheckAvailability()
    {
        if (m_AbilityAsset != null && m_AbilityAsset.IsAvailable())
        {
            gameObject.SetActive(true);
        }
    }

    private void UpdateManaStatus(int mana)
    {
        if (m_AbilityAsset == null || m_ButtonAbility == null || m_ManaCostText == null)
            return;

        if (mana >= m_AbilityAsset.m_ManaCost != m_ButtonAbility.interactable)
        {
            m_ButtonAbility.interactable = !m_ButtonAbility.interactable;
            m_ManaCostText.color = m_ButtonAbility.interactable ? Color.white : Color.red;
        }
    }  

    private void TryUseAbility()
    {
        if (m_AbilityAsset == null || Abilities.Instance == null)
            return;

        // Проверяем хватает ли маны и вычитаем
        if (TDPlayer.Instance.TrySpendMana(m_AbilityAsset.m_ManaCost))
        {
            // Вызываем нужную абилку
            if (m_AbilityType == AbilityType.Fire)
                Abilities.Instance.UseFireAbility();
            else if (m_AbilityType == AbilityType.Time)
                Abilities.Instance.UseTimeAbility();
        }
    }

    private void OnDestroy()
    {
        TDPlayer.UnsubscribeFromManaChanged(UpdateManaStatus);
        if (m_ButtonAbility != null)
            m_ButtonAbility.onClick.RemoveListener(TryUseAbility);
        
        Upgrades.OnUpgradeApplied -= CheckAvailability;
    }
}
