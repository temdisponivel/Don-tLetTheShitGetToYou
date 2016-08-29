using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class ShitterFactory
{
    private List<SocialPosition> _socialPositionValues = new List<SocialPosition>(Enum.GetValues(typeof(SocialPosition)) as SocialPosition[]);

    private List<Sprite> _images;

    private List<Sprite> Images
    {
        get
        {
            if (_images == null)
                _images = new List<Sprite>(Resources.LoadAll<Sprite>("Textures"));

            return _images;
        }
    } 

    public List<Shitter> GenerateShitters(int quantity, float maxShitAmmount)
    {
        var result = new List<Shitter>(quantity);

        var toAdd = new List<SocialPosition>();
        for (int i = 0; i < _socialPositionValues.Count; i++)
        {
            var chance = ScriptableObjectHolder.Instance.GameConfiguration.SocialPositionByChance.Find(s => s.SocialPosition == _socialPositionValues[i]).Chance;
            for (int j = 0; j < chance; j++)
            {
                toAdd.Add(_socialPositionValues[i]);
            }
        }
        
        var random = new Random();
        for (int i = 0; i < quantity; i++)
        {
            var shitter = new Shitter();

            var name = ScriptableObjectHolder.Instance.GameDatabase.Names[random.Next(0, ScriptableObjectHolder.Instance.GameDatabase.Names.Count)];

            shitter.Name = name;
            shitter.ShitAmmount = .5f + (float)(random.NextDouble() * maxShitAmmount);
            shitter.SpriteShitter = Images[random.Next(0, Images.Count)];
            shitter.SocialPosition = toAdd[random.Next(0, toAdd.Count)];

            result.Add(shitter);
        }

        return result;
    }
}