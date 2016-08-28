

using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameDatabaseScriptableObject))]
public class GameDatabaseScriptableObjectCustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        //DrawDefaultInspector();

        if (GUILayout.Button("Bake dialogs id"))
        {
            var gameDatabase = (GameDatabaseScriptableObject)target;

            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("public enum DialogId");
            stringBuilder.AppendLine("{");
            for (int i = 0; i < gameDatabase.AllDialogs.Count; i++)
            {
                stringBuilder.AppendLine(string.Format("{0},", gameDatabase.AllDialogs[i].Id));
            }
            stringBuilder.AppendLine("}");

            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (path == "")
            {
                path = "Assets";
            }
            else if (Path.GetExtension(path) != "")
            {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }
            
            File.WriteAllText(path + "DialogId.cs", stringBuilder.ToString());

            AssetDatabase.Refresh();
        }
    }
}
