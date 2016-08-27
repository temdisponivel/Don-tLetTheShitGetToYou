using UnityEngine;
using System.Collections;

public class HouseLevelManager : MonoBehaviour
{
    public static HouseLevelManager Instance = null;
    public HouseGuiManager HouseGuiManager;

    void Awake()
    {
        Instance = this;

        string messageOfTheDay = ScriptableObjectHolder.Instance.GameDatabaseScriptableObject.WakeupMessages[Random.Range(0, ScriptableObjectHolder.Instance.GameDatabaseScriptableObject.WakeupMessages.Count)];
        HouseGuiManager.PlayMenssageOfTheDay(messageOfTheDay);
    }
}
