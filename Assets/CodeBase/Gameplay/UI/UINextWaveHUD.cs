using UnityEngine;
using UnityEngine.UI;

public class UINextWaveHUD : MonoBehaviour
{
    [SerializeField] Text m_BonusAmount;
    private EnemyWavesManager m_EnemyManager;
    private float m_TimeToNextWave;

    private void OnEnable()
    {
        EnemyWave.OnPrepareNextWave += OnPrepareNextWaveHandler;
    }

    private void OnDisable()
    {
        EnemyWave.OnPrepareNextWave -= OnPrepareNextWaveHandler;
    }

    private void Start()
    {
        m_EnemyManager = FindObjectOfType<EnemyWavesManager>();
    }

    private void OnPrepareNextWaveHandler(float time)
    {
        m_TimeToNextWave = Mathf.Max(0f, time);
    }

    public void CallWave()
    {
        foreach (var manager in FindObjectsOfType<EnemyWavesManager>())
        {
            if (!manager.IsCompleted)
                manager.ForceNextWave();
        }
    }

    private void Update()
    {
        var bonus = Mathf.Max(0, Mathf.CeilToInt(m_TimeToNextWave));
        m_BonusAmount.text = bonus.ToString();
        m_TimeToNextWave -= Time.deltaTime;
    }
}
