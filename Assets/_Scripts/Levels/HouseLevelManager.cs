using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class HouseLevelManager : MonoBehaviour
{
    public static HouseLevelManager Instance = null;
    public HouseGuiManager HouseGuiManager;

    private bool _showingInitialMessage = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        HouseGuiManager.OnGoToWorkCallback += OnButtonCallback;

        if (GameManager.Instance.ShownInitialMessage)
        {
            ShowMessageOfTheDay();
        }
        else
        {
            _showingInitialMessage = true;
            HouseGuiManager.ChangeTextOfButton("Ok");

            string messageOfTheDay = string.Format(ScriptableObjectHolder.Instance.GameDatabase.InitialMessage,
                ScriptableObjectHolder.Instance.GameConfiguration.EndGameDay);
            HouseGuiManager.ShowMessageOfTheDay(messageOfTheDay, null, true);
        }
    }

    private void OnButtonCallback()
    {
        if (_showingInitialMessage)
        {
            GameManager.Instance.ShownInitialMessage = true;
            _showingInitialMessage = false;
            HouseGuiManager.ChangeTextOfButton("Go to work");
            ShowMessageOfTheDay();
        }
        else
        {
            if (GameManager.Instance.GameEnded)
            {
                GameManager.Instance.Reset();
                GameManager.Instance.LoadTitleScene();
            }
            else
            {
                GameManager.Instance.StartNewDay();
            }
        }
    }

    private void ShowMessageOfTheDay()
    {
        if (GameManager.Instance.GameEnded)
        {
            SoundManager.Instance.StopAll();
            var messageForEnd =
                ScriptableObjectHolder.Instance.GameDatabase.MessageForEndGame.Find(
                    m => m.EndOption == GameManager.Instance.End).Message;

            SoundManager.Instance.PlayAudio(AudioId.Horror);
            AudioId sound = AudioId.Ambiance;
            switch (GameManager.Instance.End)
            {
                case EndOptions.Win:
                    SoundManager.Instance.PlayAudio(AudioId.Win);
                    break;
                case EndOptions.ShitterInTheQueue:
                case EndOptions.ShitOverflow:
                    SoundManager.Instance.PlayAudio(AudioId.FartLong, AudioId.FartMedium, AudioId.FartShort);
                    break;
                case EndOptions.Killed:
                    sound = AudioId.Stab;
                    break;
                case EndOptions.DenyRoialty:
                    sound = AudioId.Hanging;
                    break;
                case EndOptions.DenyCleric:
                    sound = AudioId.Guillotine;
                    break;
            }

            HouseGuiManager.ShowMessageOfTheDay(messageForEnd, () =>
            {
                if (sound != AudioId.Ambiance)
                {
                    SoundManager.Instance.Stop(AudioId.Horror);
                    SoundManager.Instance.PlayAudio(sound);
                }
            }, false);
            return;
        }

        bool hideDiary = true;

        string messageOfTheDay;
        var dataBase = ScriptableObjectHolder.Instance.GameDatabase;
        if (!GameManager.Instance.CanDenyCleric && !GameManager.Instance.ClericMessageShowed)
        {
            SoundManager.Instance.PlayAudio(AudioId.Threat);
            messageOfTheDay = dataBase.LetterFromCleric;
            GameManager.Instance.ClericMessageShowed = true;
        }
        else if (GameManager.Instance.ThreatCount > 0)
        {
            messageOfTheDay = dataBase.MessageForHouseThrashed;
            SoundManager.Instance.PlayAudio(AudioId.Threat);
        }
        else
        {
            SoundManager.Instance.PlayAudio(AudioId.Ambiance);
            messageOfTheDay = dataBase.WakeupMessages[Random.Range(0, dataBase.WakeupMessages.Count)];
            hideDiary = false;
        }

        HouseGuiManager.ShowMessageOfTheDay(messageOfTheDay, null, hideDiary);
    }
}