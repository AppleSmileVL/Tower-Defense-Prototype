using System;
using UnityEngine;

public class MapCompletion : SingletonBase<MapCompletion>
{
    [SerializeField] private LevelScore[] m_CompletionData;
    
    private int m_TotalScore;
    public int TotalScore => m_TotalScore;

    public const string filename = "completion.dat";

    public static void ResetSavedData()
    {
        Saver<LevelScore[]>.Reset(filename);    
    }

    public static void ClearData()
    {
        if (Instance != null)
        {
            Instance.m_CompletionData = new LevelScore[0];
            Instance.m_TotalScore = 0;
        }
    }

    [Serializable]
    private class LevelScore
    {
        public LevelProperties m_Level;
        public int m_Score;
    }

    private void Awake()
    {
        Init();
        Saver<LevelScore[]>.TryLoad(filename, ref m_CompletionData);
        foreach (var levelScore in m_CompletionData)
        {
            m_TotalScore += levelScore.m_Score;
        }

        ValidateCompletionData();
    }

    private void ValidateCompletionData()
    {
        if (m_CompletionData == null || m_CompletionData.Length == 0)
        {
            Debug.LogError("MapCompletion: completionData пуста или не инициализирована!");
            return;
        }

        Debug.Log($"=== MapCompletion Validation ===");

        for (int i = 0; i < m_CompletionData.Length; i++)
        {
            var item = m_CompletionData[i];
            if (item.m_Level == null)
            {
                Debug.LogError($"[{i}] NULL - LevelProperties не назначена!");
            }
            else
            {
                Debug.Log($"[{i}] {item.m_Level.LevelTitle} (Scene: {item.m_Level.SceneName}) - Score: {item.m_Score}");
            }
        }
    }

    private LevelProperties m_CurrentLevel;
    public void SetCurrentLevel(LevelProperties level)
    {
        m_CurrentLevel = level;
    }

    public static void SaveLevelResult(int levelScore)
    {
        if (Instance == null)
        {
            Debug.LogError("MapCompletion: Instance не инициализирован!");
            return;
        }

        LevelProperties currentLevel = Instance.m_CurrentLevel;

        if (currentLevel == null)
        {
            Debug.LogError("MapCompletion: m_CurrentLevel не установлен!");
            return;
        }

        Instance.SaveResult(currentLevel, levelScore);
    }

    private void SaveResult(LevelProperties currentLevel, int levelScore)
    {
        foreach (var item in m_CompletionData)
        {
            if (item.m_Level == currentLevel)
            {
                if (levelScore > item.m_Score)
                {
                    int scoreDifference = levelScore - item.m_Score;
                    item.m_Score = levelScore;
                    m_TotalScore += scoreDifference;
                    Saver<LevelScore[]>.Save(filename, m_CompletionData);
                }
                break;
            }
        }
    }

    public int GetLevelScore(LevelProperties m_Level)
    {
        foreach (var data in m_CompletionData)
        {
            if (data.m_Level == m_Level)
            {
                return data.m_Score;
            }
        }
        return 0;
    }
}
