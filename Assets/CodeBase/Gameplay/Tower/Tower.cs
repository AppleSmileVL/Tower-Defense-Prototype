using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private float m_Radius;
    [SerializeField] private UpgradeAsset m_RadiusUpgrade;

    private Turret[] m_Turrets;
    private Destructible m_Target;

    private void Awake()
    {
        var level = Upgrades.GetUpgradeLevel(m_RadiusUpgrade);
        m_Radius += level * 0.5f;
        print($"Tower Upgrade Level: {level}, Radius: {m_Radius}");
    }

    public void Use( TowerAsset asset)
    {
        GetComponentInChildren<SpriteRenderer>().sprite = asset.m_TowerSprite;
        GetComponentInChildren<UIBuildSite>().SetBuildableTower(asset.m_UpgradeTo);
        var sr = transform.Find("View").GetComponent<SpriteRenderer>();
        sr.color = asset.color;
        sr.sprite = asset.m_TowerSprite;
        m_Turrets = GetComponentsInChildren<Turret>();

        foreach (var turet in m_Turrets)
        {
            turet.AssignLoadout(asset.m_TurretProperties);
        }
    }

    private void Update()
    {
        if (m_Target)
        {
            Vector2 targetVector = m_Target.transform.position - transform.position; 
            if (targetVector.magnitude <= m_Radius) 
            {
                foreach (var turret in m_Turrets)
                {
                    turret.transform.up = targetVector.normalized;
                    turret.Fire();
                }
            }
            else
            {
                m_Target = null;
            }
        }
        else
        {
            var enter = Physics2D.OverlapCircle(transform.position, m_Radius);

            if (enter)
            {
                m_Target = enter.transform.root.GetComponent<Destructible>();
            }
        }
    }

    #region Gizmos
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

        Gizmos.DrawWireSphere(transform.position, m_Radius);
    }
#endif
#endregion
}
