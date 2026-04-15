using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private TurretMode m_Mode;
    public TurretMode Mode => m_Mode;

    [SerializeField] private TurretProperties m_TurretProperties;
    private float m_RerfireTimer;
    public bool CanFire => m_RerfireTimer <= 0f;

    private EnemyBase m_Ship;

    private void Start()
    {
        m_Ship = transform.root.GetComponent<EnemyBase>();
    }

    private void Update()
    {
        if (m_RerfireTimer > 0)
            m_RerfireTimer -= Time.deltaTime;
        else if (Mode == TurretMode.Auto)
            Fire(); 
    }

    public void Fire()
    {
        if (m_TurretProperties == null)
            return;

        if (m_RerfireTimer > 0)
            return;

        if (m_Ship)
        {
            if (m_Ship.DrawEnergy((int)m_TurretProperties.EnergyUsage) == false)
                return;

            if (m_Ship.DrawAmmo((int)m_TurretProperties.AmmoUsage) == false)
                return;
        }

        Projectile projectile = Instantiate(m_TurretProperties.ProjectilePrefab).GetComponent<Projectile>();
        projectile.Use(m_TurretProperties);
        projectile.transform.position = transform.position;
        projectile.transform.up = transform.up;
        projectile.SetParentShooter(m_Ship);

        m_RerfireTimer = m_TurretProperties.RateOfFire;
    }

    public void AssignLoadout(TurretProperties properties)
    {
        if (m_Mode != properties.Mode) // проверяем, совпадает ли режим турели с режимом переданных свойств
            return;                    // если не совпадает, выходим из метода

        m_RerfireTimer = 0;
        m_TurretProperties = properties; // присваиваем переданные свойства турели
    }
}
