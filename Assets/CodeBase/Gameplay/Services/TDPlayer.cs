using System;
using UnityEngine;

public class TDPlayer : Player
{
    [SerializeField] private int m_Mana;
    [SerializeField] private int m_MaxMana = 100;

    public int Mana => m_Mana;
    public int MaxMana => m_MaxMana;

    private float m_ManaRegenTimer;
    private const float MANA_REGEN_INTERVAL = 1f;

    private void Update()
    {
        m_ManaRegenTimer -= Time.deltaTime;
        if (m_ManaRegenTimer <= 0)
        {
            m_ManaRegenTimer = MANA_REGEN_INTERVAL;
            if (m_Mana < m_MaxMana)
            {
                m_Mana++;
                OnManaChanged?.Invoke(m_Mana);
            }
        }
    }

    private void Awake()
    {
        Init();
        ApplyHealthUpgrade();
    }

    public static new TDPlayer Instance
    {
        get
        {
            var instance = Player.Instance as TDPlayer;
            if (instance == null)
            {
                instance = FindObjectOfType<TDPlayer>();
            }
            return instance;
        }
    }

    private static event Action<int> OnManaChanged;

    public static void SubscribeToManaChanged(Action<int> act)
    {
        OnManaChanged += act;
        if (Instance != null)
            act(Instance.Mana);
    }

    public static void UnsubscribeFromManaChanged(Action<int> act)
    {
        OnManaChanged -= act;
    }

    private static event Action<int> OnGoldUpdate;

    public static void SubscribeToGoldUpdate(Action<int> act)
    {
        OnGoldUpdate += act;
        if (Instance != null)
            act(Instance.m_Gold);
    }

    public static void UnsubscribeFromGoldUpdate(Action<int> act)
    {
        OnGoldUpdate -= act;
    }

    public static event Action<int> OnLifeUpdate;

    public static void SubscribeToLifeUpdate(Action<int> act)
    {
        OnLifeUpdate += act;
        if (Instance != null)
            act(Instance.NumLives);
    }

    public static void UnsubscribeFromLifeUpdate(Action<int> act)
    {
        OnLifeUpdate -= act;
    }

    [SerializeField] private int m_Gold = 0;

    public void ChangeGold(int m_Change)
    {
        m_Gold += m_Change;
        OnGoldUpdate?.Invoke(m_Gold);
    }

    public void ChangeMana(int m_Change)
    {
        m_Mana = Mathf.Min(m_Mana + m_Change, m_MaxMana);
        OnManaChanged?.Invoke(m_Mana);
    }

    public void AddMana(int m_Num)
    {
        m_Mana = Mathf.Min(m_Mana + m_Num, m_MaxMana);
        OnManaChanged?.Invoke(m_Mana);
    }

    public bool TrySpendMana(int m_Cost)
    {
        if (m_Mana >= m_Cost)
        {
            m_Mana -= m_Cost;
            OnManaChanged?.Invoke(m_Mana);
            return true;
        }
        return false;
    }

    public void ReduceLife(int m_Change)
    {
        TakeDamage(m_Change);
        OnLifeUpdate?.Invoke(NumLives);
    }

    private void Start()
    {
        m_ManaRegenTimer = MANA_REGEN_INTERVAL;
        m_MaxMana = Mathf.Max(1, m_MaxMana);
        m_Mana = Mathf.Clamp(m_Mana, 0, m_MaxMana);

        OnGoldUpdate?.Invoke(m_Gold);
        OnLifeUpdate?.Invoke(NumLives);
        OnManaChanged?.Invoke(m_Mana);
    }

    [SerializeField] private Tower towerPrefab;

    public void TryBuild(TowerAsset m_TowerAsset, Transform m_BuildSite)
    {
        ChangeGold(-m_TowerAsset.m_GoldCost);
        
        var tower = Instantiate(towerPrefab, m_BuildSite.position, Quaternion.identity);
        tower.Use(m_TowerAsset);
        Destroy(m_BuildSite.gameObject);
    }
}
