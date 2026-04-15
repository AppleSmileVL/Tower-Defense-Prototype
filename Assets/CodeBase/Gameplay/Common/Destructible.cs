using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Уничтожаемый обьект на сцене. То что может иметь хитпоинты.
/// </summary>
public class Destructible : Entity
{
    #region Properties

    /// <summary>
    /// Обьект игнорирует поврждения.
    /// </summary>
    [SerializeField] private bool m_Indestrictible;
    public bool IsIndestrictible => m_Indestrictible;

    /// <summary>
    /// Стартовое количество хитпоинтов.
    /// </summary>
    [SerializeField] private int m_HitPoints;
    public int MaxHitPoints => m_HitPoints;

    /// <summary>
    /// Текущие хитпоинты.
    /// </summary>
    [SerializeField]private int m_CurrentHitPoints;
    public int HitPoints => m_CurrentHitPoints;

    [Header("Effect")]
    [SerializeField] private ParticleSystem m_DeathEffect;

    #endregion

    #region Unity Events

    protected virtual void Awake()
    {
        FindDeathExplosion();
    }

    protected virtual void Start()
    {
        m_CurrentHitPoints = m_HitPoints;

        transform.SetParent(null);
    }

    #endregion

    #region Public API

    /// <summary>
    /// Применение урона к обьекту.
    /// </summary>
    /// param name="damage"> Количество урона.</param>
    public void ApplyDamage(int damage)
    {
        m_CurrentHitPoints -= damage;
        if (m_CurrentHitPoints <= 0)
            OnDeath();

    }
    #endregion

    public void FindDeathExplosion()
    {
        if (m_DeathEffect == null)
        {
            m_DeathEffect = GetComponentInChildren<ParticleSystem>(true);
        }
    }

    /// <summary>
    /// Переопределяемое событие уничтожения обьекта, кигда хитпоинты достигают и ниже нуля.
    /// </summary>
    protected virtual void OnDeath()
    {
        if (m_DeathEffect == null)
        {
            FindDeathExplosion();
        }

        if (m_DeathEffect != null)
        {
            ParticleSystem explosionInstance = Instantiate(m_DeathEffect, transform.position, transform.rotation);
            explosionInstance.transform.parent = null;
            explosionInstance.Play();


            Destroy(explosionInstance.gameObject,
                    explosionInstance.main.duration +
                    explosionInstance.main.startLifetime.constantMax);
        }

        m_EventOnDeath?.Invoke();
        Destroy(gameObject);
    }

    private static HashSet<Destructible> m_AllDestructibles; 

    public static IReadOnlyCollection<Destructible>AllDestructibles => m_AllDestructibles;

    protected virtual void OnEnable()
    {
        if(m_AllDestructibles == null)
           m_AllDestructibles = new HashSet<Destructible>();

        m_AllDestructibles.Add(this);
    }

    protected virtual void OnDestroy()
    {
        m_AllDestructibles.Remove(this);
    }

    public const int TeamIdNeural = 0;

    [SerializeField] private int m_TeamId;
    public int TeamId => m_TeamId;

    [SerializeField] private UnityEvent m_EventOnDeath;
    public UnityEvent EventOnDeath => m_EventOnDeath;

    [SerializeField] private int m_Mana;
    public int ScoreValue => m_Mana;

    protected void Use(EnemyAsset asset)
    {
        m_HitPoints = asset.m_HP;
        m_Mana = asset.m_RestoreMana;
    }
}
