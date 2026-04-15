using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultPanel : MonoBehaviour
{
    private const string PassedText = "Victory";
    private const string LoseText = "Defeat";
    private const string RestartText = "Restart";
    private const string NextText = "Next level";
    private const string WorldMapSceneName = "World_Map";
    private const string MainMenuSceneName = "Main_Menu";

    [SerializeField] private Text m_ResultText;
    [SerializeField] private Text m_ButtonNextText;
    [SerializeField] private Button m_ButtonNext;
    [SerializeField] private Sound m_WinSound = Sound.PlayerWin;
    [SerializeField] private Sound m_LoseSound = Sound.PlayerLose;

    private bool m_LevelPassed = false;

    private void Start()
    {
        gameObject.SetActive(false);
        LevelController.Instance.LevelLost += OnLevelLost;
        LevelController.Instance.LevelPassed += OnLevelPassed;

        if (LevelController.Instance.IsLevelComplete)
        {
            OnLevelPassed();
        }
        else if (LevelController.Instance.IsLevelFailed)
        {
            OnLevelLost();
        }
    }

    private void OnDestroy()
    {
        if (LevelController.Instance != null)
        {
            LevelController.Instance.LevelLost -= OnLevelLost;
            LevelController.Instance.LevelPassed -= OnLevelPassed;
        }
    }

    private void OnLevelPassed()
    {
        gameObject.SetActive(true);

        m_LevelPassed = true;
        m_ResultText.text = PassedText;
        m_ResultText.color = Color.green;
        m_WinSound.Play();

        if(LevelController.Instance.HasNextLevel == true)
        {
            m_ButtonNextText.text = NextText;
        }
        else
        {
            m_ButtonNext.interactable = false;
        }
    }

    private void OnLevelLost()
    {
        gameObject.SetActive(true);
        m_ResultText.text = LoseText;
        m_ResultText.color = Color.red;
        m_ButtonNextText.text = RestartText;
        m_LoseSound.Play();
    }

    public void OnButtonNextAction()
    {
        gameObject.SetActive(false);

        if(m_LevelPassed == true)
        {
            LevelController.Instance.LoadNextLevel();
        }
        else
        {
            LevelController.Instance.RestartLevel();
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(MainMenuSceneName);
    }
    public void LoadWorldMap()
    {
        SceneManager.LoadScene(WorldMapSceneName);
    }
}
