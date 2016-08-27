using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    #region Properties

    private float _shitAmmount;

    public float ShitAmmount
    {
        get
        {
            return _shitAmmount;
        }
        set
        {
            _shitAmmount = value;

            if (_shitAmmount >= ScriptableObjectHolder.Instance.GameConfiguration.MaxShitAmmount)
            {
                Lose();
                return;
            }

            if (OnShitAmmountChanges != null)
                OnShitAmmountChanges();
        }
    }

    public int CurrentDay { get; set; }
    public TimeSpan DateTime;
    
    public Image BackgroundFade;

    public readonly List<Shitter> AllTimeShitters = new List<Shitter>();

    public ShitterFactory ShitterFactory = new ShitterFactory();

    #endregion

    #region Events

    public event Action OnStartNewDay;
    public event Action OnEndDay;
    public event Action OnWin;
    public event Action OnLose;
    public event Action OnUpdateTime;
    public event Action OnShitAmmountChanges;

    #endregion

    #region End game

    public void Win()
    {
        if (OnWin != null)
            OnWin();
    }

    public void Lose()
    {
        if (OnLose != null)
            OnLose();
    }

    #endregion

    #region Days

    public void StartNewDay()
    {
        StartCoroutine(StartDay());
        if (OnStartNewDay != null)
            OnStartNewDay();
    }

    public void EndDay()
    {
        ShitAmmount = ScriptableObjectHolder.Instance.GameConfiguration.MaxShitAmmountDecreasePerDay * CurrentDay;

        if (OnEndDay != null)
            OnEndDay();
    }

    #endregion

    #region Coroutines

    private IEnumerator StartDay()
    {
        DateTime = new TimeSpan(CurrentDay++, 9, 0, 0);
        int count = 60 * 8; // 60 minutes times 8 hours
        while (count-- > 0)
        {
            yield return new WaitForSeconds(1f); //every seconds represents on minute in game
            DateTime += new TimeSpan(0, 0, 1, 0);
            if (OnUpdateTime != null)
                OnUpdateTime();
        }
        EndDay();
    }

    #endregion

    #region Scenes

    public void GoToWork()
    {
        StartDay();
        LoadWorkScene();
    }

    public void LoadWorkScene()
    {
        DOTween.ToAlpha(() => BackgroundFade.color, (color) =>
        {
            BackgroundFade.color = color;
        }, 1f, .5f).OnComplete(() =>
        {
            SceneManager.LoadScene("Work");

            DOTween.ToAlpha(() => BackgroundFade.color, (color) =>
            {
                BackgroundFade.color = color;
            }, 0f, .5f);
        });
    }

    public void LoadHouseScene()
    {
        DOTween.ToAlpha(() => BackgroundFade.color, (color) =>
        {
            BackgroundFade.color = color;
        }, 1f, .5f).OnComplete(() =>
        {
            SceneManager.LoadScene("House");

            DOTween.ToAlpha(() => BackgroundFade.color, (color) =>
            {
                BackgroundFade.color = color;
            }, 0f, .5f);
        });
    }

    #endregion

    #region Shitters

    public List<Shitter> GetShittersForToday()
    {
        var gameConfiguration = ScriptableObjectHolder.Instance.GameConfiguration;

        int quantityToGenerate = (int) Mathf.Ceil(gameConfiguration.ShittersToGeneratePerDay * Random.Range(.7f, 1.3f));
        var newShitters = ShitterFactory.GenerateShitters(quantityToGenerate);
        AllTimeShitters.AddRange(newShitters);

        int quantityToReturn = (int)Mathf.Ceil(gameConfiguration.ShittersPerDay + (gameConfiguration.ShittersPerDayIncrease*CurrentDay));

        var result = new List<Shitter>(quantityToReturn);
        for (int i = 0; i < quantityToReturn; i++)
        {
            result.Add(AllTimeShitters[Random.Range(0, AllTimeShitters.Count)]);
        }

        return result;
    }

    #endregion
}
