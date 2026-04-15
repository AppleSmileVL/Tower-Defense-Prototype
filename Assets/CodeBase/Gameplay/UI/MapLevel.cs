using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapLevel : MonoBehaviour
{
    [SerializeField]private LevelProperties m_Level;
    public LevelProperties Level => m_Level;
    [SerializeField]private RectTransform m_FinalResultPanel;
    [SerializeField] private Image[] m_FinalResultImage;
    [SerializeField] private Text m_LevelTitle;
    [SerializeField] private Image m_PreviewImage;

    public bool IsComplete { get { return gameObject.activeSelf && m_FinalResultPanel.gameObject.activeSelf; } }

    private void Start()
    {
        if (m_Level == null) return;

        m_PreviewImage.sprite = m_Level.PreviewImage;
        m_LevelTitle.text = m_Level.LevelTitle;
        
    }

    public void LoadLevel()
    {
        Debug.Log("LoadLevel called for: " + (m_Level != null ? m_Level.LevelTitle : "null"));
        if (m_Level == null)
        {
            Debug.LogError("Level data is not set!");
            return;
        }
        SceneManager.LoadScene(m_Level.SceneName);
    }

    public void Initialise()
    {
        var score = MapCompletion.Instance.GetLevelScore(m_Level);
        m_FinalResultPanel.gameObject.SetActive(score > 0);
        for (int i = 0; i < score; i++)
        {
            m_FinalResultImage[i].color = Color.white;
        }
    }
}
