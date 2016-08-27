using System.Collections.Generic;
using UnityEngine;

public class ShitterFactory
{
    public List<Shitter> GenerateShitters(int quantity)
    {
        var result = new List<Shitter>(quantity);

        for (int i = 0; i < quantity; i++)
        {
            var shitter = new Shitter();

            var name = ScriptableObjectHolder.Instance.GameDatabaseScriptableObject.Names[Random.Range(0, ScriptableObjectHolder.Instance.GameDatabaseScriptableObject.Names.Count)];
            var story = ScriptableObjectHolder.Instance.GameDatabaseScriptableObject.Stories[Random.Range(0, ScriptableObjectHolder.Instance.GameDatabaseScriptableObject.Stories.Count)];

            shitter.Name = name;
            shitter.Story = story;
            shitter.ShitAmmount = Random.Range(1, 30);

            Debug.Log(shitter);
        }

        return result;
    }
}