using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class HouseLevelManager : MonoBehaviour
{
    public static HouseLevelManager Instance = null;
    public HouseGuiManager HouseGuiManager;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (GameManager.Instance.ShownInitialMessage)
        {
            ShowMessageOfTheDay();
        }
        else
        {
            string messageOfTheDay = string.Format(ScriptableObjectHolder.Instance.GameDatabase.InitialMessage,
                ScriptableObjectHolder.Instance.GameConfiguration.EndGameDay);
            HouseGuiManager.ShowMessage(messageOfTheDay, ShowMessageOfTheDay);
        }
    }

    private void ShowMessageOfTheDay()
    {
        if (GameManager.Instance.GameEnded)
        {
            SoundManager.Instance.StopAll();
            var timeToWait = SoundManager.Instance.Sounds.Find(s => s.AudioId == AudioId.Horror).AudioSource.clip.length;
            switch (GameManager.Instance.End)
            {
                case EndOptions.Win:
                    break;
                case EndOptions.ShitterInTheQueue:
                case EndOptions.ShitOverflow:
                    SoundManager.Instance.PlayAudio(AudioId.FartLong, AudioId.FartMedium, AudioId.FartShort);
                    break;
                case EndOptions.Killed:
                    StartCoroutine(PlaySoundWithHorror(AudioId.Stab, timeToWait));
                    break;
                case EndOptions.DenyRoialty:
                    StartCoroutine(PlaySoundWithHorror(AudioId.Hanging, timeToWait));
                    break;
                case EndOptions.DenyCleric:
                    StartCoroutine(PlaySoundWithHorror(AudioId.Guillotine, timeToWait));
                    break;
            }

            HouseGuiManager.ShowMessageOfTheDay(ScriptableObjectHolder.Instance.GameDatabase.MessageForEndGame.Find(m => m.EndOption == GameManager.Instance.End).Message);
            return;
        }

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
        }

        HouseGuiManager.ShowMessageOfTheDay(messageOfTheDay);
    }

    private IEnumerator PlaySoundWithHorror(AudioId sound, float timeToWait)
    {
        SoundManager.Instance.PlayAudio(AudioId.Horror);
        yield return new WaitForSeconds(timeToWait);
        SoundManager.Instance.PlayAudio(sound);
    }
}
