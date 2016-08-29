using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ScriptableObjectHolder))]
public class ScriptableObjectHolderCustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        var scriptableObjectHolder = (ScriptableObjectHolder)target;

        if (GUILayout.Button("Bake references"))
        {
            scriptableObjectHolder.GameDatabase = Resources.Load<GameDatabaseScriptableObject>("ScriptableObjects/GameDatabse");
            scriptableObjectHolder.GameConfiguration = Resources.Load<GameConfigurationScriptableObject>("ScriptableObjects/GameConfiguration");
            scriptableObjectHolder.ShitterTextures = Resources.LoadAll<Texture>("");
        }
        EditorUtility.SetDirty(target);
    }
}