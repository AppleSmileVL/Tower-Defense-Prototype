using UnityEngine;
using UnityEditor;
using System;
#if UNITY_EDITOR
using UnityEditor.PackageManager;
#endif

[RequireComponent(typeof(TDPatrolController))]
[RequireComponent(typeof(Destructible))]
public class Enemy : MonoBehaviour
{
    public enum ArmorType
    {
        Base = 0,
        Mage = 1
    }
    private static Func<int, TDProjectile.DamageType, int, int>[] ArmorDamageFunctions =
    {
        (int power, TDProjectile.DamageType type, int armor) =>
        {   // Ѕазова€ брон€: —нижает любой урон на значение брони
            switch (type)
            {
                case TDProjectile.DamageType.Magic: return power; // ћагическа€ брон€ не снижает магический урон
                default: return Math.Max(power - armor, 1); 
            }
        },
        (int power, TDProjectile.DamageType type, int armor) =>
        {   // ћагическа€ брон€: —нижает физический урон на значение брони, магический урон на половину значени€ брони
            if (TDProjectile.DamageType.Magic == type)
                armor = armor / 2; // ћагова€ брон€ снижает магический урон вдвое меньше
            return Math.Max(power - armor, 1);
            
        }
    };

    [SerializeField] private int m_Damage = 1;
    [SerializeField] private int m_Gold = 10;
    [SerializeField] private int m_RestoreMana = 10;
    [SerializeField] private int m_Armor = 1;
    [SerializeField] private ArmorType m_ArmorType;

    private Destructible m_Destructible;

    private void Awake()
    {
        m_Destructible = GetComponent<Destructible>();
    }

    public event Action OnEnd;

    private void OnDestroy()
    {
        OnEnd?.Invoke();
    }

    public void Use(EnemyAsset asset)
    {
        var sr = transform.Find("View").GetComponent<SpriteRenderer>();
        sr.color =  asset.color;
        sr.transform.localScale = new Vector3(asset.spriteScale.x, asset.spriteScale.y, 1);
        sr.GetComponent<Animator>().runtimeAnimatorController = asset.m_Animations;

        GetComponent<EnemyBase>().Use(asset);

        GetComponentInChildren<CircleCollider2D>().radius = asset.m_Radius;

        m_Damage = asset.m_Damage;
        m_Armor = asset.m_Armor;
        m_ArmorType = asset.m_ArmorType;
        m_Gold = asset.m_Gold;
        m_RestoreMana = asset.m_RestoreMana;
    }

    public void DamagePlayer()
    {

        var m_TDPlayer = FindObjectOfType<TDPlayer>();
        
        if (m_TDPlayer != null)
        {
            m_TDPlayer.ReduceLife(m_Damage);
            return;
        }

        Debug.LogWarning($"Player not found - cannot apply {m_Damage} damage.");
    }

    public void GivePlayerGold()
    {

        var m_TDPlayer = Player.Instance as TDPlayer;
       
        m_TDPlayer = FindObjectOfType<TDPlayer>();
        
        if (m_TDPlayer != null)
        {
            m_TDPlayer.ChangeGold(m_Gold);
            return;
        }

        Debug.LogWarning($"TDPlayer not found - cannot give {m_Gold} gold to player.");
    }

    public void GivePlayerMana()
    {
        var m_TDPlayer = Player.Instance as TDPlayer;

        m_TDPlayer = FindObjectOfType<TDPlayer>();

        if (m_TDPlayer != null)
        {
            m_TDPlayer.ChangeMana(m_RestoreMana);
            return;
        }
    }

    public void TakeDamage(int damage, TDProjectile.DamageType damageType )
    {
        m_Destructible.ApplyDamage(ArmorDamageFunctions[(int)m_ArmorType] (damage, damageType, m_Armor));
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(Enemy))]
public class EnemyInspector: Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EnemyAsset a = EditorGUILayout.ObjectField(null, typeof(EnemyAsset), false) as EnemyAsset;

        if (a)
        {
            (target as Enemy).Use(a);
        }
    }
}
#endif
