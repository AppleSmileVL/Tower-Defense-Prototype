using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Abilities : SingletonBase<Abilities>
{
    private void Awake()
    {
        Init();
    }

    [Serializable]
    public class FireAbility
    {
        [SerializeField] private UpgradeAsset m_DamageUpgrade;
        [SerializeField] private int m_Damage;
        [SerializeField] private Color m_TargetingCircleColor;

        public void Use()
        {
            UIClickProtection.Instance.Activate((Vector2 v) =>
            {
                Vector3 position = v;
                position.z = -Camera.main.transform.position.z;
                position = Camera.main.ScreenToWorldPoint(position);

                int upgradeDamage = m_Damage;
                if (m_DamageUpgrade != null)
                {
                    int upgradeLevel = Upgrades.GetUpgradeLevel(m_DamageUpgrade);
                    upgradeDamage += upgradeLevel * 5;
                    print($"Fire Ability Upgrade Level: {upgradeLevel}, Damage: {upgradeDamage}");
                }

                foreach (var collider in Physics2D.OverlapCircleAll(position, 2))
                {
                    if (collider.transform.parent.TryGetComponent<Destructible>(out var destructible))
                    {
                        destructible.ApplyDamage(upgradeDamage);
                    }
                }
                
            });
        }
    }

    [Serializable]
    public class TimeAbility
    {   
        [SerializeField] private UpgradeAsset m_DurationUpgrade;
        [SerializeField] private float m_Duration;
        [SerializeField] private float m_Cooldown;


        public void Use()
        {
            void SlowTime(Enemy enemy)
            {
                enemy.GetComponent<EnemyBase>()?.HalfMaxLinearVelocity();
            }

            foreach (var enemy in FindObjectsOfType<EnemyBase>())
                enemy.HalfMaxLinearVelocity();

            EnemyWavesManager.OnEnemySpawned += SlowTime;

            float upgradeDuration = m_Duration;
            if (m_DurationUpgrade != null)
            {
                int upgradeLevel = Upgrades.GetUpgradeLevel(m_DurationUpgrade);
                upgradeDuration += upgradeLevel * 5.0f;
                print($"Time Ability Upgrade Level: {upgradeLevel}, Duration: {upgradeDuration}");
            }

            IEnumerator Restore()
            {
                yield return new WaitForSeconds(upgradeDuration);
                foreach (var enemy in FindObjectsOfType<EnemyBase>())
                    enemy.RestoreMaxLinearVelocity();
                EnemyWavesManager.OnEnemySpawned -= SlowTime;
            }
            
            Instance.StartCoroutine(Restore());
            
            IEnumerator TimeAbilityButton()
            {
                Instance.m_TimeButton.interactable = false;
                yield return new WaitForSeconds(m_Cooldown);
                Instance.m_TimeButton.interactable = true;
            }
            Instance.StartCoroutine(TimeAbilityButton());
        }
    }

    [SerializeField] private Button m_TimeButton;
    [SerializeField] private Image m_TargetingCircle;
    [SerializeField] private FireAbility m_FireAbility;
    public void UseFireAbility() => m_FireAbility.Use();

    [SerializeField] private TimeAbility m_TimeAbility;
    public void UseTimeAbility() => m_TimeAbility.Use();
}
