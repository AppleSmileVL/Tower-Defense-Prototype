using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{
    [SerializeField] private GameObject m_PausePanel;

    private const string WorldMapSceneName = "World_Map";
    private const string MainMenuSceneName = "Main_Menu";

    private bool isPaused = false;

    private void Start()
    {
        m_PausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            m_PausePanel.SetActive(true);

        }
        else
        {
            Time.timeScale = 1f;
            m_PausePanel.SetActive(false);
        }
    }

    public void ContinueGame()
    {
        TogglePause();
    }

    public void RestartLevel()
    {
        LevelController.Instance.RestartLevel();
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(MainMenuSceneName);
    }

    public void LoadWorldMap()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(WorldMapSceneName);
    }
}
