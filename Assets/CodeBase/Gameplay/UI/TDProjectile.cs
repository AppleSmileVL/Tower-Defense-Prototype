using UnityEngine;

public class TDProjectile : Projectile
{
    public enum DamageType
    {
        Base,
        Magic,
        Explosion
    }

    [SerializeField] private DamageType m_DamageType;
    [SerializeField] private Sound m_ShotSound;

    private void Start()
    {
        m_ShotSound.Play();
    }

    protected override void OnHit(RaycastHit2D hit)
    {
        var enemy = hit.collider.transform.root.GetComponent<Enemy>();

        if (enemy != null)
        {
            enemy.TakeDamage(m_Damage, m_DamageType);
        }
    }
}
