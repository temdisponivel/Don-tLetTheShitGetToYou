using UnityEngine;
using System.Collections;

public class HouseLevelManager : MonoBehaviour
{
    public static HouseLevelManager Instance = null;
    public HouseGuiManager HouseGuiManager;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (GameManager.Instance.ShownInitialMessage)
        {
            ShowMessageOfTheDay();
        }
        else
        {
            string messageOfTheDay = string.Format(ScriptableObjectHolder.Instance.GameDatabase.InitialMessage,
                ScriptableObjectHolder.Instance.GameConfiguration.EndGameDay);
            HouseGuiManager.ShowMessage(messageOfTheDay, ShowMessageOfTheDay);
        }
    }

    private void ShowMessageOfTheDay()
    {
        if (GameManager.Instance.GameEnded)
        {
            HouseGuiManager.ShowMessageOfTheDay(ScriptableObjectHolder.Instance.GameDatabase.MessageForEndGame.Find(m => m.EndOption == GameManager.Instance.End).Message);
            return;
        }

        string messageOfTheDay;
        var dataBase = ScriptableObjectHolder.Instance.GameDatabase;
        if (!GameManager.Instance.CanDenyCleric && !GameManager.Instance.ClericMessageShowed)
        {
            messageOfTheDay = dataBase.LetterFromCleric;
            GameManager.Instance.ClericMessageShowed = true;
        }
        else if (GameManager.Instance.ThreatCount > 0)
        {
            messageOfTheDay = dataBase.MessageForHouseThrashed;
        }
        else
        {
            messageOfTheDay = dataBase.WakeupMessages[Random.Range(0, dataBase.WakeupMessages.Count)];
        }

        HouseGuiManager.ShowMessageOfTheDay(messageOfTheDay);
    }
}
