using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldMapMenu : MonoBehaviour
{
    [SerializeField] private GameObject m_MainPanel;
    [SerializeField] private GameObject m_UpgradePanel;
    [SerializeField] private GameObject m_GoMainButton;

    private const string MainMenuSceneName = "Main_Menu";

    private bool isPaused = false;

    private void Start()
    {
        m_UpgradePanel.SetActive(false);
        m_MainPanel.SetActive(false);
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (m_UpgradePanel.activeSelf)
            {
                Back();
            }
            else
            {
                TogglePanel();
            }
        }
    }

    public void TogglePanel()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            m_MainPanel.SetActive(true);
        }
        else
        {
            m_MainPanel.SetActive(false);
        }
    }

    public void ContinueGame()
    {
        TogglePanel();
    }

    public void OpenUpgradePanel()
    {
        m_UpgradePanel.SetActive(true);
        m_MainPanel.SetActive(false);
        m_GoMainButton.SetActive(false);
    }

    public void Back()
    {
        m_UpgradePanel.SetActive(false);
        m_MainPanel.SetActive(true);
        m_GoMainButton.SetActive(true);

    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(MainMenuSceneName);
    }
}
