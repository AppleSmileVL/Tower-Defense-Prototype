using UnityEngine;
using UnityEngine.UI;

public class UITextUpdate : MonoBehaviour
{
    public enum UpdateSource { Gold, Life, Mana }
    public UpdateSource Source = UpdateSource.Gold;
    private Text m_Text;

    private void Start()
    {
        m_Text = GetComponent<Text>();

        if (m_Text == null)
        {
            enabled = false;
            return;
        }

        switch (Source)
        {
            case UpdateSource.Gold:
                TDPlayer.SubscribeToGoldUpdate(UpdateText);
                break;
            case UpdateSource.Life:
                TDPlayer.SubscribeToLifeUpdate(UpdateText);
                break;
            case UpdateSource.Mana:
                TDPlayer.SubscribeToManaChanged(UpdateText);
                break;
        }
    }

    private void OnDestroy()
    {
        switch (Source)
        {
            case UpdateSource.Gold:
                TDPlayer.UnsubscribeFromGoldUpdate(UpdateText);
                break;
            case UpdateSource.Life:
                TDPlayer.UnsubscribeFromLifeUpdate(UpdateText);
                break;
            case UpdateSource.Mana:
                TDPlayer.UnsubscribeFromManaChanged(UpdateText);
                break;
        }
    }

    private void UpdateText(int value)
    {
        m_Text.text = value.ToString();
    }
}
