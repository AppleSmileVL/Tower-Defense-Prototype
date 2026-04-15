using UnityEngine;

public enum TurretMode
{
    Primary,
    Secondary,
    Auto
}

[CreateAssetMenu]
public sealed class TurretProperties : ScriptableObject // sealed это параметр изолированный класс , от которого нельзя наследоваться
{
    [SerializeField] private TurretMode m_Mode;
    public TurretMode Mode => m_Mode;

    [SerializeField] private  Projectile m_ProjectilePrefab;
    public Projectile ProjectilePrefab => m_ProjectilePrefab;

    [SerializeField] private float m_RateOfFire;
    public float RateOfFire => m_RateOfFire;

    [SerializeField] private int m_Damage;
    public int Damage => m_Damage;

    [SerializeField] private Sprite m_ProjectileView;
    public Sprite ProjectileView => m_ProjectileView;

    [SerializeField] private Color color = Color.white;
    public Color Color => color;

    private float m_EnergyUsage;
    public float EnergyUsage => m_EnergyUsage;

    private float m_AmmoUsage;
    public float AmmoUsage => m_AmmoUsage;
}
