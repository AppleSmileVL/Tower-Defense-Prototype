using UnityEngine;

public class Projectile : Entity
{
    [SerializeField] private float m_Velocity; 
    public float Velocity => m_Velocity;

    [SerializeField] private float m_Lifetime;
    public float Lifetime => m_Lifetime;

    [SerializeField] protected int m_Damage;
    public int Damage => m_Damage;

    [SerializeField] private GameObject m_ImpactEffectPrefab;
    public GameObject ImpactEffectPrefab => m_ImpactEffectPrefab;

    [SerializeField] private UpgradeAsset m_DamageUpgrade;

    protected private float m_Timer;

    public void Use(TurretProperties turret, int damageUpgradeLevel = -1)
    {
        var sr = transform.Find("View").GetComponent<SpriteRenderer>();
        sr.sprite = turret.ProjectileView;
        sr.color = turret.Color;
        m_Damage = turret.Damage;

        if (m_DamageUpgrade != null)
        {
            int upgradeLevel = Upgrades.GetUpgradeLevel(m_DamageUpgrade);
            m_Damage += upgradeLevel * 5;
            print($"Turret Upgrade Level: {upgradeLevel}, Projectile damage: {m_Damage}");
        }
    }

    private void Update()
    {
        float stepLenth = Time.deltaTime * m_Velocity; 
        Vector2 step = transform.up * stepLenth; 

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, stepLenth); 

        if (hit)
        {
            OnHit(hit);
            OnProjectileLifeEnd(hit.collider, hit.point);
        }

        m_Timer += Time.deltaTime;

        if (m_Timer > m_Lifetime)
            Destroy(gameObject);
        
        transform.position += new Vector3(step.x, step.y, 0); 
    }
    
    protected virtual void OnHit(RaycastHit2D hit)
    {
        var destructible = hit.collider.transform.root.GetComponent<Destructible>();
        if (destructible != null)
        {
            destructible.ApplyDamage(m_Damage);
        }
    }

    private void OnProjectileLifeEnd(Collider2D col, Vector2 pos) 
    {
        Destroy(gameObject);
    }

    protected Destructible m_Parent; 

    public void SetParentShooter(Destructible parent) 
    {
        m_Parent = parent;
    }
}
