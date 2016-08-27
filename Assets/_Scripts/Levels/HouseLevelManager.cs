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
        if (GameManager.Instance.ShownInitialMessage || true)
        {
            ShowMessageOfTheDay();
        }
        else
        {
            string messageOfTheDay = ScriptableObjectHolder.Instance.GameDatabaseScriptableObject.InitialMessage;
            HouseGuiManager.ShowMessage(messageOfTheDay, ShowMessageOfTheDay);
        }
    }

    private void ShowMessageOfTheDay()
    {
        string messageOfTheDay = ScriptableObjectHolder.Instance.GameDatabaseScriptableObject.WakeupMessages[Random.Range(0, ScriptableObjectHolder.Instance.GameDatabaseScriptableObject.WakeupMessages.Count)];
        HouseGuiManager.ShowMenssageOfTheDay(messageOfTheDay);
    }
}
