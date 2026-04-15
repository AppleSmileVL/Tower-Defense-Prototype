using UnityEngine;
using UnityEngine.UI;

public class TimerDisplay : MonoBehaviour
{
    [SerializeField] private Text m_TimerText;

    private void Update()
    {
        if (LevelController.Instance != null)
        {
            float remainingTime = LevelController.Instance.RemainingTime;
            m_TimerText.text = FormatTime(remainingTime);
        }
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        return $"{minutes:00}:{seconds:00}";
    }
}