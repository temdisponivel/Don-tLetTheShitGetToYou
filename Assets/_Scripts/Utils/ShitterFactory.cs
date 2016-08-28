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
        var socialPositionValues = new List<SocialPosition>(Enum.GetValues(typeof(SocialPosition)) as SocialPosition[]);

        var toAdd = new List<SocialPosition>();
        for (int i = 0; i < socialPositionValues.Count; i++)
        {
            var chance = ScriptableObjectHolder.Instance.GameConfiguration.SocialPositionByChance.Find(s => s.SocialPosition == socialPositionValues[i]).Chance;
            for (int j = 0; j < chance; j++)
            {
                toAdd.Add(socialPositionValues[i]);
            }
        }

        socialPositionValues.AddRange(toAdd);

        var maxShit = ScriptableObjectHolder.Instance.GameConfiguration.MaxShitAmmount / (quantity * 1f);
        for (int i = 0; i < quantity; i++)
        {
            var shitter = new Shitter();

            var name = ScriptableObjectHolder.Instance.GameDatabase.Names[Random.Range(0, ScriptableObjectHolder.Instance.GameDatabase.Names.Count)];

            shitter.Name = name;
            shitter.ShitAmmount = Random.Range(.5f, maxShit);
            shitter.SpriteShitter = images[i % images.Length];
            shitter.SocialPosition = socialPositionValues[Random.Range(0, socialPositionValues.Count)];

            result.Add(shitter);
        }

        return result;
    }
}