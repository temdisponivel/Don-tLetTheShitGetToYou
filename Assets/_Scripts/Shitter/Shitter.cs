using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[Serializable]
public class Shitter
{
    private static int _currentShitterId;

    public int Id;
    public string Name;
    public string Story;
    public int ShitAmmount;

    public readonly Dictionary<int, bool> AllTimeDecisions = new Dictionary<int, bool>();

    public float TimeShitting
    {
        get { return ShitAmmount * 0.5f; }
    }

    public SocialPosition SocialPosition;

    public Shitter()
    {
        Id = _currentShitterId++;
    }

    public string GetMessageForPlayer()
    {
        return string.Empty;
    }

    public string Accepted()
    {
        AllTimeDecisions[GameManager.Instance.CurrentDay] = true;
        return string.Empty;
    }

    public string Denied()
    {
        AllTimeDecisions[GameManager.Instance.CurrentDay] = false;
        return string.Empty;
    }

}
