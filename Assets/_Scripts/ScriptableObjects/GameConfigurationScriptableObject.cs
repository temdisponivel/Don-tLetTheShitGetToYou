using UnityEngine;
using System.Collections;

public class GameConfigurationScriptableObject : ScriptableObject
{
    public float MaxShitAmmount;
    public float MaxShitAmmountIncreasePerDay;
    public int ShittersToGeneratePerDay;
    public float ShittersPerDay;
    public float ShittersPerDayIncrease;
    public float MaxShitPerShitter;
}
