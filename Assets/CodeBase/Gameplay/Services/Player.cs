using UnityEngine;

public class Player : SingletonBase<Player>
{
    /*
    public void Construct(FollowCamera followCamera, ShipInputController shipInputController, Transform spawnPoint)
    {
        m_FollowCamera = followCamera;
        m_ShipInputController = shipInputController;
        m_SpawnPoint = spawnPoint;
    }
    */

    private EnemyBase m_EnemyPrefab;
    [SerializeField] private int m_NumLives;

    private int m_NumKills;
    public EnemyBase ActiveEnemy => m_EnemyPrefab;
    public int NumKills => m_NumKills;
    public int NumLives => m_NumLives;

    [SerializeField] private UpgradeAsset m_HealthUpgrade;

    private void Start()
    {
        if (m_EnemyPrefab)
        {
            m_EnemyPrefab.EventOnDeath.AddListener(OnPlayerCampDeath);
        }
    }

    public void ApplyHealthUpgrade()
    {
        var level = Upgrades.GetUpgradeLevel(m_HealthUpgrade);
        m_NumLives += level * 5;
        print($"Player lives upgrade level: {level}, Lives: {NumLives}.");
    }

    private void OnPlayerCampDeath()
    {
        m_NumLives--;
        if (m_NumLives < 0)
        {
            m_NumLives = 0;
        } 
    }

    public void AddKill()
    {
        m_NumKills += 1;
    }

    protected void TakeDamage(int m_Damage)
    {
        m_NumLives -= m_Damage;
        if (m_NumLives <= 0)
        {
            m_NumLives = 0;
        }
    }

    public void AddLife(int m_Amount)
    {
        m_NumLives += m_Amount;
    }
}
