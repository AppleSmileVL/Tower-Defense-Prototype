using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWave: MonoBehaviour 
{
    public static Action<float> OnPrepareNextWave;
    [Serializable]
    private class Squad
    {
        public EnemyAsset m_Asset;
        public int m_Count;
    }
    [Serializable]
    private class PathGroup
    {
        public Squad[] m_Squads;
    }
    
    [SerializeField] private PathGroup[] m_Groups;
    
    [SerializeField] private float m_PrepareTime = 10f;

    public float GetRemainTime() { return Mathf.Max(0f, m_PrepareTime - Time.time); }

    void Awake()
    {
        enabled = false;
    }

    private event Action OnWaveReady;
    public void Prepare(Action m_SpawnEnemies)
    {
        m_PrepareTime += Time.time;
        OnPrepareNextWave?.Invoke(GetRemainTime());
        enabled = true;
        OnWaveReady += m_SpawnEnemies;
    }
    private void Update()
    {
        if (Time.time >= m_PrepareTime)
        {
            enabled = false;
            OnWaveReady?.Invoke();
            // OnWaveReady = null;
        }
    }

    public IEnumerable<(EnemyAsset asset, int count, int pathIndex)> EnumarateSquads()
    {
        for (int i = 0; i < m_Groups.Length; i++)
        {
            foreach (var squad in m_Groups[i].m_Squads)
            {
                yield return (squad.m_Asset, squad.m_Count, i);
            }
        }
    }
    [SerializeField] private EnemyWave nextWave;
    public EnemyWave PrepareNext(Action spawnEnemies)
    {
        OnWaveReady -= spawnEnemies;
        if (nextWave) nextWave.Prepare(spawnEnemies);
        return nextWave;
    }
}