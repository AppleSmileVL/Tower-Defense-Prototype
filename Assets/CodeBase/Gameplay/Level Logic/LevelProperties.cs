using UnityEngine;

[CreateAssetMenu]
public class LevelProperties : ScriptableObject
{
    [SerializeField] private string m_SceneName;
    [SerializeField] private string m_LevelTitle;
    [SerializeField] private Sprite m_PreviewImage;
    [SerializeField] private LevelProperties m_NextLevel;

    public string SceneName => m_SceneName;
    public string LevelTitle => m_LevelTitle;
    public Sprite PreviewImage => m_PreviewImage;
    public LevelProperties NextLevel => m_NextLevel;

}
