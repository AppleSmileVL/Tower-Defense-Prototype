using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelController : SingletonBase<LevelController>
{
    private const string WorldMapSceneName = "World_Map";

    public event UnityAction LevelPassed;
    public event UnityAction LevelLost;

    [SerializeField] private LevelProperties m_LevelProperties;
    [SerializeField] private LevelCondition[] m_Conditions;
    [SerializeField] private float m_ReferenceTime;

    private bool m_IsLevelComplete;
    private bool m_IsLevelFailed;
    private float m_LevelTime;
    public float ReferenceTime => m_ReferenceTime;
    public bool IsLevelComplete => m_IsLevelComplete;
    public bool IsLevelFailed => m_IsLevelFailed;
    public bool HasNextLevel => m_LevelProperties.NextLevel != null;
    public float RemainingTime => Mathf.Max(m_ReferenceTime - Time.time, 0);
    public float LevelTime => m_LevelTime;

    private int levelScore = 3;

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        if (MapCompletion.Instance != null)
        {
            MapCompletion.Instance.SetCurrentLevel(m_LevelProperties);
        }
        else
        {
            Debug.LogError("LevelController: MapCompletion.Instance не найден!");
        }
        
        m_LevelTime += Time.time;
        m_ReferenceTime += Time.time;

        bool isFirstCall = true;
        void LifeScoreChange(int _)
        {
            if (isFirstCall)
            {
                isFirstCall = false;
                return;
            }
            levelScore -= 1;
            TDPlayer.OnLifeUpdate -= LifeScoreChange;
        }
        TDPlayer.OnLifeUpdate += LifeScoreChange;
    }

    private void Update()
    {
        if (m_IsLevelComplete == false && m_IsLevelFailed == false)
        {
            m_LevelTime += Time.deltaTime;
            CheckLevelConditions();
        }
        
        var player = GetPlayer();
        if (player != null && player.NumLives == 0 && m_IsLevelFailed == false)
        {
            Lose();
        }
    }

    private TDPlayer GetPlayer()
    {
        return TDPlayer.Instance != null ? TDPlayer.Instance : UnityEngine.Object.FindObjectOfType<TDPlayer>();
    }

    private void CheckLevelConditions()
    {
        int numComplete = 0;

        for (int i = 0; i < m_Conditions.Length; i++)
        {
            if (m_Conditions[i].IsCompleted == true)
            {
                numComplete++;
            }
        }

        if (numComplete == m_Conditions.Length)
        {
            m_IsLevelComplete = true;

            Win();
        }
    }

    private void StopLevelActivity()
    {
        foreach (var enemy in FindObjectsOfType<Enemy>())
        {
            enemy.GetComponent<EnemyBase>().enabled = false;
            enemy.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            DestroyImmediate(enemy.gameObject);
        }

        static void DisableAll<T>() where T : MonoBehaviour
        {
            foreach (var obj in FindObjectsOfType<T>())
            {
                obj.enabled = false;
            }
        }
        DisableAll<EnemyWave>();
        DisableAll<Projectile>();
        DisableAll<Tower>();
        DisableAll<UINextWaveHUD>();
        DisableAll<TimerDisplay>();
    }

    private void LevelCompleted()
    {
        StopLevelActivity();
        if (m_ReferenceTime <= Time.time)
        {
            levelScore -= 1;
        }
    }

    private void Lose()
    {
        m_IsLevelFailed = true;
        LevelCompleted();
        LevelLost?.Invoke();
    }

    private void Win()
    {
        m_IsLevelComplete = true;
        LevelCompleted();
        print($"Level Complete! Score: {levelScore}"); 
        MapCompletion.SaveLevelResult(levelScore);
        LevelPassed?.Invoke();
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1.0f;
        if (HasNextLevel == true) 
        {
            SceneManager.LoadScene(m_LevelProperties.NextLevel.SceneName);
        }
        else
        {
            SceneManager.LoadScene(WorldMapSceneName);
        }
    }

    public void RestartLevel()
    {
        Time.timeScale = 1.0f;
        DestroyImmediate(gameObject);
        SceneManager.LoadScene(m_LevelProperties.SceneName);
    }
}
