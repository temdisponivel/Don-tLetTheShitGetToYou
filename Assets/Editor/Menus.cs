using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;


public static class Menus
{
    [MenuItem("DLSGTY/Create shitter database")]
    public static void CreateShitter()
    {
        CreateAsset<GameDatabaseScriptableObject>();
    }

    [MenuItem("DLSGTY/Create game configuration")]
    public static void CreateGameConfiguration()
    {
        CreateAsset<GameConfigurationScriptableObject>();
    }

    /// <summary>
    //	This makes it easy to create, name and place unique new ScriptableObject asset files.
    /// </summary>
    public static void CreateAsset<T>() where T : ScriptableObject
    {
        T asset = ScriptableObject.CreateInstance<T>();

        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (path == "")
        {
            path = "Assets";
        }
        else if (Path.GetExtension(path) != "")
        {
            path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeof(T).ToString() + ".asset");

        AssetDatabase.CreateAsset(asset, assetPathAndName);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}