using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using Random = UnityEngine.Random;

[Serializable]
public class Shitter
{
    private static int _currentShitterId;

    public int Id;
    public string Name;
    public string Story;
    public int ShitAmmount;
    public Sprite SpriteShitter;
    public SocialPosition SocialPosition;

    public readonly Dictionary<int, bool> AllTimeDecisions = new Dictionary<int, bool>();

    public float TimeShitting
    {
        get { return ShitAmmount * 0.5f; }
    }

    public Shitter()
    {
        Id = _currentShitterId++;
    }

    public string GetMessageForPlayer()
    {
        var gameDatabase = ScriptableObjectHolder.Instance.GameDatabaseScriptableObject;
        string message;
        if (AllTimeDecisions.ContainsKey(GameManager.Instance.CurrentDay))
        {
            message = gameDatabase.TwiceADayMessages[Random.Range(0, gameDatabase.TwiceADayMessages.Count)];
        }
        else
        {
            message = gameDatabase.Stories[Random.Range(0, gameDatabase.Stories.Count)];
        }
        return message;
    }

    public string Accepted()
    {
        AllTimeDecisions[GameManager.Instance.CurrentDay] = true;
        var gameDatabase = ScriptableObjectHolder.Instance.GameDatabaseScriptableObject;
        return gameDatabase.AcceptedReplies[Random.Range(0, gameDatabase.AcceptedReplies.Count)];
    }

    public string Denied()
    {
        AllTimeDecisions[GameManager.Instance.CurrentDay] = false;
        var gameDatabase = ScriptableObjectHolder.Instance.GameDatabaseScriptableObject;
        return gameDatabase.DenyedReplies[Random.Range(0, gameDatabase.DenyedReplies.Count)];
    }

}
