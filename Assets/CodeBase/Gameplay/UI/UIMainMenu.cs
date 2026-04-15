using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private Button m_ContinueButton;
    private void Start()
    {
        m_ContinueButton.interactable = FileHandler.HasFile(MapCompletion.filename);
    }
    public void NewGame()
    {
        // ќчищаем файлы
        FileHandler.Reset(MapCompletion.filename);
        FileHandler.Reset(Upgrades.filename);
        
        // ”ничтожаем старые синглтоны
        if (MapCompletion.Instance != null)
        {
            Destroy(MapCompletion.Instance.gameObject);
        }
        if (Upgrades.Instance != null)
        {
            Destroy(Upgrades.Instance.gameObject);
        }
        
        SceneManager.LoadScene(1);
    }

    public void Continue()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
