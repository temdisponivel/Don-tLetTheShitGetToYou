using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameConfigurationScriptableObject : ScriptableObject
{
    public float MaxShitAmmount;
    public float MaxShitAmmountIncreasePerDay;
    public int ShittersPerDayIncrease;
    public float MaxShitPerShitter;
    public int EndGameDay;
    public int HoursPerDay;

    public List<SocialPositionChanceTuple> SocialPositionByChance;
}
