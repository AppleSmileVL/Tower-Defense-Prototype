using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBase : Destructible
{
    [SerializeField] private Sprite m_PreviewImage;
    public Sprite PreviewImage => m_PreviewImage;

    /// <summary>
    /// Масса для автоматической установки в Rigidbody2D.
    /// </summary>
    [Header("Enemy Movement")]
    [SerializeField] private float m_Mass;

    /// <summary>
    /// Толкающая сила вперед
    /// </summary>>
    [SerializeField] private float m_Thrust;

    /// <summary>
    /// Вращающая сила
    /// </summary>>
    [SerializeField] private float m_Mobility;

    /// <summary>
    /// Максимальная линейная скорость
    /// </summary>>
    [SerializeField] private float m_MaxLinearVelocity;
    private float m_OriginalMaxLinearVelocity;
    private bool m_IsOriginalMaxLinearVelocityStored;

    public void HalfMaxLinearVelocity() 
    {
        if (m_IsOriginalMaxLinearVelocityStored == false)
        {
            m_OriginalMaxLinearVelocity = m_MaxLinearVelocity;
            m_IsOriginalMaxLinearVelocityStored = true;
        }

        m_MaxLinearVelocity /= 2f; 
    }

    public void RestoreMaxLinearVelocity() 
    { 
        if (m_IsOriginalMaxLinearVelocityStored)
        {
            m_MaxLinearVelocity = m_OriginalMaxLinearVelocity;
            m_IsOriginalMaxLinearVelocityStored = false;
        }
    }

    /// <summary>
    /// Максимальгная вращающая скорость. Градусы в секунду.
    /// </summary>>
    [SerializeField] private float m_MaxAngularVelocity;

    /// <summary>
    /// Сохраненная ссылка на Rigidbody2D
    /// </summary>>
    private Rigidbody2D m_Rigid;

    #region Public API

    /// <summary>
    /// Управление линейной тягой -1.0 до +1.0
    /// </summary>>
    public float TrustControl { get; set; }

    /// <summary>
    /// Управление вращательной тягой -1.0 до +1.0
    /// </summary>>
    public float TorqueControl { get; set; }

    private SpriteRenderer spriteRenderer;

    #endregion

    #region Unity Events

    protected override void Start()
    {
        base.Start();
        
        m_Rigid= GetComponent<Rigidbody2D>();
        m_Rigid.mass = m_Mass;
        
        m_Rigid.inertia = 1;

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

    }

    private void FixedUpdate()
    {
        UpdateRigidBody();
    }

    #endregion

    /// <summary>
    /// Метод добавления сил движения
    /// </summary>
    private void UpdateRigidBody()
    {
        m_Rigid.AddForce(TrustControl * m_Thrust * transform.up * Time.fixedDeltaTime, ForceMode2D.Force); // Добавляем силу тяги

        m_Rigid.AddForce(-m_Rigid.velocity * (m_Thrust / m_MaxLinearVelocity) * Time.fixedDeltaTime, ForceMode2D.Force); // Добавляем силу торможения пропорционально скорости

        m_Rigid.AddTorque(TorqueControl * m_Mobility * Time.fixedDeltaTime, ForceMode2D.Force); // Добавляем крутящий момент

        m_Rigid.AddTorque(-m_Rigid.angularVelocity * (m_Mobility / m_MaxAngularVelocity) * Time.fixedDeltaTime, ForceMode2D.Force); // Добавляем торможение вращения пропорционально угловой скорости
    }

    private Turret[] m_Turrets;

    public void Fire(TurretMode mode)
    {
        return;
    }

    private int m_MaxEnergy;
    private int m_MaxAmmo;

    private float m_PrimaryEnergy;
    private float m_SecondaryAmmo;

    public void AddEnergy(int energy)
    {
        m_PrimaryEnergy = Mathf.Clamp(m_PrimaryEnergy + energy, 0, m_MaxEnergy);
    }

    public void AddAmmo(int ammo)
    {
        m_SecondaryAmmo = Mathf.Clamp(m_SecondaryAmmo + ammo, 0, m_MaxAmmo);
    }

    public bool DrawEnergy(int count)
    {
        return true;
    }

    public bool DrawAmmo(int count)
    {
        return true;
    }

    public void AssignWeapon(TurretProperties props)
    {
        for (int i = 0; i < m_Turrets.Length; i++)
        {
            m_Turrets[i].AssignLoadout(props); 
        }
    }

    public new void Use(EnemyAsset asset)
    {
        m_MaxLinearVelocity = asset.m_moveSpeed;
        base.Use(asset);
    }
}
