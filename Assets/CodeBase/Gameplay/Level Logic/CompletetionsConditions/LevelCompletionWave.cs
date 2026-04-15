using UnityEngine;

public class LevelCompletionWave : LevelCondition
{
    private bool _completed = false;

    private void Awake()
    {
        var managers = FindObjectsOfType<EnemyWavesManager>();
        if (managers == null || managers.Length == 0)
        {
            _completed = true;
            return;
        }

        int total = managers.Length;
        int done = 0;

        foreach (var wavesManager in managers)
        {
            if (wavesManager.IsCompleted)
            {
                done++;
                continue;
            }

            wavesManager.OnAllWavesCompleted += () =>
            {
                done++;
                if (done >= total)
                {
                    _completed = true;
                }
            };
        }

        if (done >= total)
        {
            _completed = true;
        }
    }

    public override bool IsCompleted
    {
        get
        {
            return _completed;
        }
    }
}
