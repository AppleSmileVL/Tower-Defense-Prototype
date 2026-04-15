using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : SingletonBase<SoundPlayer>
{
    [SerializeField] private Sounds m_Sounds;
    [SerializeField] private AudioClip m_BackgroundMusic;
    private AudioSource m_AudioSource;

    private void Awake()
    {
        Init();
        m_AudioSource = GetComponent<AudioSource>();
        Instance.m_AudioSource.clip = m_BackgroundMusic;
        Instance.m_AudioSource.Play();
    }

    public void Play(Sound sound)
    {
        m_AudioSource.PlayOneShot(m_Sounds[sound]);
    }
}
