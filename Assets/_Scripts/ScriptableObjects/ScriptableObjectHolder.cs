using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectHolder : Singleton<ScriptableObjectHolder>
{
    public GameDatabaseScriptableObject GameDatabase;
    public GameConfigurationScriptableObject GameConfiguration;
    public Texture[] ShitterTextures;
}