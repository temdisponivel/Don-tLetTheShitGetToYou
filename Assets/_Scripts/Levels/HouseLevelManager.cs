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
#if !UNITY_EDITOR
        string messageOfTheDay = ScriptableObjectHolder.Instance.GameDatabaseScriptableObject.InitialMessage;
            HouseGuiManager.ShowMessage(messageOfTheDay, ShowMessageOfTheDay);
#endif
        ShowMessageOfTheDay();
    }

    private void ShowMessageOfTheDay()
    {
        if (GameManager.Instance.GameEnded)
        {
            HouseGuiManager.ShowMessage(ScriptableObjectHolder.Instance.GameDatabaseScriptableObject.MessageForEndGame.Find(m => m.EndOption == GameManager.Instance.End).Message,
                () =>
                {
                    GameManager.Instance.LoadTitleScene();
                });

            return;
        }

        string messageOfTheDay;
        if (!GameManager.Instance.CanDenyCleric && !GameManager.Instance.ClericMessageShowed)
        {
            messageOfTheDay = ScriptableObjectHolder.Instance.GameDatabaseScriptableObject.LetterFromCleric;
            GameManager.Instance.ClericMessageShowed = true;
        }
        else if (GameManager.Instance.ThreatCount > 0)
        {
            messageOfTheDay = ScriptableObjectHolder.Instance.GameDatabaseScriptableObject.MessageForHouseThrashed;
        }
        else
        {
            messageOfTheDay =
                ScriptableObjectHolder.Instance.GameDatabaseScriptableObject.WakeupMessages[
                    Random.Range(0, ScriptableObjectHolder.Instance.GameDatabaseScriptableObject.WakeupMessages.Count)];
        }

        HouseGuiManager.ShowMenssageOfTheDay(messageOfTheDay);
    }
}
