using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
    public enum SpawnMode
    {
        Start,
        Loop
    }

    [SerializeField] private CircleArea m_Area;

    [SerializeField] private SpawnMode m_SpawnMode;

    [SerializeField] private int m_NumSpawns;

    [SerializeField] private float m_RespawnTime;

    private float m_Timer;

    protected abstract GameObject GenerateSpawnedEntity();

    private void Start()
    {
        if (m_SpawnMode == SpawnMode.Start)
        {
            Spawn();
        }

        m_Timer = m_RespawnTime;
    }

    private void Update()
    {

        if (m_Timer > 0)
            m_Timer -= Time.deltaTime;

        if (m_SpawnMode == SpawnMode.Loop && m_Timer < 0)
        {
            Spawn();
            m_Timer = m_RespawnTime;
        }
    }

    private void Spawn()
    {
        for (int i = 0; i < m_NumSpawns; i++)
        {
            var e = GenerateSpawnedEntity();
            e.transform.position = m_Area.GetRandomInsideZero();
        }
    }
}
