using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectHolder : Singleton<ScriptableObjectHolder>
{
    public GameDatabaseScriptableObject GameDatabaseScriptableObject;
    public GameConfigurationScriptableObject GameConfiguration;
    public Texture[] ShitterTextures;
}