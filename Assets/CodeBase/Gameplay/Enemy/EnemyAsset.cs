using UnityEngine;

[CreateAssetMenu]
public sealed class EnemyAsset : ScriptableObject
{
    [Header("Outfit")]
    public Color color = Color.white;
    public Vector2 spriteScale = new Vector2(2,2);
    public RuntimeAnimatorController m_Animations;

    [Header("Game Settings")]
    public float m_moveSpeed = 1;
    public int m_HP = 10;
    public int m_Armor = 0;
    public int m_RestoreMana = 1;
    public float m_Radius = 0.20f;
    public int m_Damage = 1;
    public int m_Gold = 10;
    public Enemy.ArmorType m_ArmorType;
}
