using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ScriptableObjectHolder))]
public class ScriptableObjectHolderCustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        var scriptableObjectHolder = (ScriptableObjectHolder) target;

        if (GUILayout.Button("Bake references"))
        {
            scriptableObjectHolder.GameDatabaseScriptableObject = Resources.FindObjectsOfTypeAll<GameDatabaseScriptableObject>()[0];
            scriptableObjectHolder.GameConfiguration = Resources.FindObjectsOfTypeAll<GameConfigurationScriptableObject>()[0];
        }

        EditorUtility.SetDirty(target);
    }
}
