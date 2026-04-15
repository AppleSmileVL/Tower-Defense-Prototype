using UnityEngine;
using System;

public class EnemyWavesManager : MonoBehaviour
{
    [SerializeField] private Path[] m_Path;
    [SerializeField] private Enemy m_EnemyPrefab;
    [SerializeField] private EnemyWave m_CurrentWaves;

    public static event Action<Enemy> OnEnemySpawned;

    public event Action OnAllWavesCompleted;
    private int m_ActiveEnemyCount;

    private bool m_AllWavesCompleted = false;

    public bool IsCompleted => m_AllWavesCompleted || (m_CurrentWaves == null && m_ActiveEnemyCount == 0);

    private void RecordEnemyDead() 
    {
        if (--m_ActiveEnemyCount == 0)
        {
            ForceNextWave();
        }
    }

    private void Start()
    {
        if (m_CurrentWaves)
        {
            m_CurrentWaves.Prepare(SpawnEnemies);
        }
        else
        {
            TryInvokeAllWavesComplete();
        }
    }

    public void ForceNextWave()
    {
        if (m_CurrentWaves)
        {
            TDPlayer.Instance.ChangeGold((int)m_CurrentWaves.GetRemainTime());
            SpawnEnemies();
        }
        else
        {
            TryInvokeAllWavesComplete();
        }
    }

    private void TryInvokeAllWavesComplete()
    {
        if (m_AllWavesCompleted) return;
        if(m_ActiveEnemyCount == 0)
        {
            m_AllWavesCompleted = true;
            OnAllWavesCompleted?.Invoke();
        }
    }

    private void SpawnEnemies() 
    {
        foreach ((EnemyAsset asset, int count, int pathIndex) in m_CurrentWaves.EnumarateSquads())
        {
            if (pathIndex < m_Path.Length)
            {
                for (int i = 0; i < count; i++)
                {
                    var enemy = Instantiate(m_EnemyPrefab,
                                            m_Path[pathIndex].StartArea.GetRandomInsideZero(),
                                            Quaternion.identity);
                    enemy.OnEnd += RecordEnemyDead;
                    enemy.Use(asset);
                    enemy.GetComponent<TDPatrolController>().SetPath(m_Path[pathIndex]);
                    m_ActiveEnemyCount += 1;
                    OnEnemySpawned?.Invoke(enemy);
                }
            }
            else
            {
                Debug.LogWarning($"Path with index {pathIndex} not found!");
            }
        }

        m_CurrentWaves = m_CurrentWaves.PrepareNext(SpawnEnemies);

        TryInvokeAllWavesComplete();
    }
}
