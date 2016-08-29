using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;

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
                EndGame(EndOptions.ShitOverflow);
                return;
            }

            if (OnShitAmmountChanges != null)
                OnShitAmmountChanges();
        }
    }

    public int CurrentDay { get; set; }

    private int _currentHour;

    public int CurrentHour
    {
        get
        {
            return _currentHour;
        }
        set
        {
            _currentHour = value;
            if (OnUpdateTime != null)
                OnUpdateTime();
        }
    }

    public Image BackgroundFade;

    public ShitterFactory ShitterFactory = new ShitterFactory();

    public bool ShownInitialMessage;

    public List<Shitter> TodaysShitters;

    public bool GameEnded = false;
    public EndOptions End = EndOptions.Win;

    public bool CanDenyCleric
    {
        get { return ClericDenyed <= 1; }
    }

    private int _clericDenyed;
    public int ClericDenyed
    {
        get
        {
            return _clericDenyed;
        }
        set
        {
            _clericDenyed = value;

            if (_clericDenyed >= 3)
            {
                EndGame(EndOptions.DenyCleric);
            }
        }
    }

    private int _threatCount;
    public int ThreatCount
    {
        get
        {
            return _threatCount;
        }
        set
        {
            _threatCount = value;

            if (_threatCount >= 2)
            {
                EndGame(EndOptions.Killed);
            }
        }
    }

    public bool ClericMessageShowed { get; set; }

    public Coroutine CurrentDayCoroutine;

    #endregion

    #region Events

    public event Action OnStartNewDay;
    public event Action OnEndDay;
    public event Action OnEnd;
    public event Action OnUpdateTime;
    public event Action OnShitAmmountChanges;
    public event Action<string> OnSceneLoad;

    #endregion

    #region Unity events

    void Start()
    {
        CurrentDay = 0;
        Shitter.BakeDialogs();
    }

    #endregion

    #region End game

    public void EndGame(EndOptions end)
    {
        End = end;
        GameEnded = true;
        if (OnEnd != null)
            OnEnd();
    }
    
    #endregion

    #region Days

    public void StartNewDay()
    {
        LoadWorkScene(() =>
        {
            CurrentDayCoroutine = StartCoroutine(StartDay());
            if (OnStartNewDay != null)
                OnStartNewDay();
        });
    }

    public void EndDay()
    {
        if (CurrentDay == ScriptableObjectHolder.Instance.GameConfiguration.EndGameDay)
        {
            EndGame(EndOptions.Win);
        }

        if (CurrentDayCoroutine != null)
            StopCoroutine(CurrentDayCoroutine);

        ShitAmmount = ScriptableObjectHolder.Instance.GameConfiguration.MaxShitAmmountIncreasePerDay * CurrentDay;
        if (OnEndDay != null)
            OnEndDay();
    }

    #endregion

    #region Coroutines

    private IEnumerator StartDay()
    {
        CurrentDay++;
        CurrentHour = 9;
        int count = ScriptableObjectHolder.Instance.GameConfiguration.HoursPerDay;
        var timeToWait = 0f;
        var random = new Random();
        TodaysShitters = GetShittersForToday();
        for (int i = 0; i < TodaysShitters.Count; i++)
        {
            timeToWait += TodaysShitters[i].TimeShitting + (float)(8 + (random.NextDouble() * 20)); //random to account for dialogs
        }

        timeToWait /= count;

        while (count-- > 0)
        {
            yield return new WaitForSeconds(timeToWait);
            CurrentHour++;
        }
        EndDay();
    }

    #endregion

    #region Scenes
    
    public void LoadWorkScene(Action callback)
    {
        DOTween.ToAlpha(() => BackgroundFade.color, (color) =>
        {
            BackgroundFade.color = color;
        }, 1f, .5f).OnComplete(() =>
        {
            SceneManager.LoadScene("Work");

            if (OnSceneLoad != null)
                OnSceneLoad("Work");

            DOTween.ToAlpha(() => BackgroundFade.color, (color) =>
            {
                BackgroundFade.color = color;
            }, 0f, .5f).OnComplete(() =>
            {
                if (callback != null)
                    callback();
            });
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

            if (OnSceneLoad != null)
                OnSceneLoad("House");

            DOTween.ToAlpha(() => BackgroundFade.color, (color) =>
            {
                BackgroundFade.color = color;
            }, 0f, .5f);
        });
    }

    public void LoadTitleScene()
    {
        DOTween.ToAlpha(() => BackgroundFade.color, (color) =>
        {
            BackgroundFade.color = color;
        }, 1f, .5f).OnComplete(() =>
        {
            SceneManager.LoadScene("Title");

            if (OnSceneLoad != null)
                OnSceneLoad("Title");

            DOTween.ToAlpha(() => BackgroundFade.color, (color) =>
            {
                BackgroundFade.color = color;
            }, 0f, .5f);
        });
    }

    #endregion

    #region Shitters

    private List<Shitter> GetShittersForToday()
    {
        var gameConfiguration = ScriptableObjectHolder.Instance.GameConfiguration;
        var random = new Random();
        int quantityToReturn = gameConfiguration.ShittersPerDayIncrease * CurrentDay;
        var maxShitAmmount = gameConfiguration.MaxShitAmmount /  (quantityToReturn * (float)(.85f + (random.NextDouble() * 1.3f)));
        return ShitterFactory.GenerateShitters(quantityToReturn, maxShitAmmount);
    }

    #endregion

    #region Reset

    public void Reset()
    {
        _shitAmmount = 0;
        CurrentDay = 0;
        CurrentHour = 0;
        TodaysShitters = new List<Shitter>();
        ClericDenyed = 0;
        ThreatCount = 0;
        ClericMessageShowed = false;
        GameEnded = false;
    }

    #endregion
}