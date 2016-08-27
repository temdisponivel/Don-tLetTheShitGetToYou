using UnityEngine;
using System.Collections;

public class GameConfigurationScriptableObject : ScriptableObject
{
    public float MaxShitAmmount;
    public float MaxShitAmmountDecreasePerDay;
    public int ShittersToGeneratePerDay;
    public float ShittersPerDay;
    public float ShittersPerDayIncrease;
}
