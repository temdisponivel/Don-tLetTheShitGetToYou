﻿using System;
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
    public int CurrentHour;
    
    public Image BackgroundFade;

    public readonly List<Shitter> AllTimeShitters = new List<Shitter>();

    public ShitterFactory ShitterFactory = new ShitterFactory();

    public bool ShownInitialMessage { get; set; }

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
        ShitAmmount = ScriptableObjectHolder.Instance.GameConfiguration.MaxShitAmmountIncreasePerDay * CurrentDay;
        LoadHouseScene();
        if (OnEndDay != null)
            OnEndDay();
    }

    #endregion

    #region Coroutines

    private IEnumerator StartDay()
    {
        CurrentDay++;
        CurrentHour = 9;
        int count = 8;
        while (count-- > 0)
        {
            yield return new WaitForSeconds(30);
            CurrentHour++;
            Debug.Log("HOUR");
            if (OnUpdateTime != null)
                OnUpdateTime();
        }
        EndDay();
    }

    #endregion

    #region Scenes

    public void GoToWork()
    {
        LoadWorkScene(StartNewDay);
    }

    public void LoadWorkScene(Action callback)
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

                if (callback != null)
                    callback();
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

    public Queue<Shitter> GetShittersForToday()
    {
        var gameConfiguration = ScriptableObjectHolder.Instance.GameConfiguration;

        int quantityToGenerate = (int) Mathf.Ceil(gameConfiguration.ShittersToGeneratePerDay * Random.Range(.7f, 1.3f));
        var newShitters = ShitterFactory.GenerateShitters(quantityToGenerate);
        AllTimeShitters.AddRange(newShitters);

        int quantityToReturn = (int)Mathf.Ceil(gameConfiguration.ShittersPerDay + (gameConfiguration.ShittersPerDayIncrease*CurrentDay));

        var result = new Queue<Shitter>(quantityToReturn);
        for (int i = 0; i < quantityToReturn; i++)
        {
            result.Enqueue(AllTimeShitters[Random.Range(0, AllTimeShitters.Count)]);
        }

        return result;
    }

    #endregion
}