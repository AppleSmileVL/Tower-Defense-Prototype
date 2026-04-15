using UnityEngine;

public enum Sound
{
    Arrow = 0,
    Canon = 1,
    Magic = 2,
    EnemyWin = 3,
    PlayerWin = 4,
    PlayerLose = 5,
    BackgroundMusic = 6 
}

public static class SoundExtensions
{
    public static void Play(this Sound sound)
    {
        SoundPlayer.Instance.Play(sound);
    }
}
