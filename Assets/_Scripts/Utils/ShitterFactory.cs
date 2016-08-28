using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShitterFactory
{
    public List<Shitter> GenerateShitters(int quantity)
    {
        var result = new List<Shitter>(quantity);

        var images = Resources.LoadAll<Sprite>("Textures");
        var socialPositionValues = Enum.GetValues(typeof (SocialPosition));

        for (int i = 0; i < quantity; i++)
        {
            var shitter = new Shitter();

            var name = ScriptableObjectHolder.Instance.GameDatabaseScriptableObject.Names[Random.Range(0, ScriptableObjectHolder.Instance.GameDatabaseScriptableObject.Names.Count)];
            var story = ScriptableObjectHolder.Instance.GameDatabaseScriptableObject.Stories[Random.Range(0, ScriptableObjectHolder.Instance.GameDatabaseScriptableObject.Stories.Count)];

            shitter.Name = name;
            shitter.Story = story;
            shitter.ShitAmmount = Random.Range(5, (int)ScriptableObjectHolder.Instance.GameConfiguration.MaxShitPerShitter);
            shitter.SpriteShitter = images[i % images.Length];
            shitter.SocialPosition = (SocialPosition)socialPositionValues.GetValue(Random.Range(0, socialPositionValues.Length));

            result.Add(shitter);
        }

        return result;
    }
}