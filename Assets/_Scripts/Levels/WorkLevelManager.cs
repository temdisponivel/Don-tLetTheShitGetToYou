using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = System.Random;

public class WorkLevelManager : MonoBehaviour
{
    public static WorkLevelManager Instance = null;
    public QueueManager QueueManager;
    public WorkGuiManager WorkGuiManager;
    private Queue<Shitter> _shitters;
    private bool _dayGoing = true;
    private Shitter _currentShitter = null;

    #region Unity events

    void Awake()
    {
        Instance = this;
        GameManager.Instance.OnStartNewDay += StartDay;
        GameManager.Instance.OnEndDay += EndDay;
    }

    void StartDay()
    {
        GameManager.Instance.OnStartNewDay -= StartDay;

        WorkGuiManager.OnAccept += OnShitterAccepted;
        WorkGuiManager.OnDeny += OnShitterDenied;

        var shitters = GameManager.Instance.TodaysShitters;
        _shitters = new Queue<Shitter>();

        for (int i = 0; i < shitters.Count; i++)
        {
            QueueManager.AddPeople(shitters[i]);
            _shitters.Enqueue(shitters[i]);
        }

        CoroutineHelper.Instance.WaitForSecondsAndCall(1f, UpdateQueue);
        SoundManager.Instance.StopAll();
        SoundManager.Instance.PlayAudio(AudioId.Horse, AudioId.BusyCity);
        SoundManager.Instance.Stop(AudioId.Ambiance);
    }

    void OnDestroy()
    {
        SoundManager.Instance.StopAll();
    }

    #endregion

    #region Game events

    void EndDay()
    {
        GameManager.Instance.OnEndDay -= EndDay;

        if (!GameManager.Instance.GameEnded && _shitters.Count > 0 && _dayGoing)
        {
            GameManager.Instance.EndGame(EndOptions.ShitterInTheQueue);
        }
        else
            GameManager.Instance.EndDay();

        _dayGoing = false;
        GameManager.Instance.LoadHouseScene();
    }

    public void ShitterArrive(Shitter shitter)
    {
        _currentShitter = shitter;
        QueueManager.RemovePeople(_currentShitter);
        WorkGuiManager.ShitterArrive(shitter, () =>
        {
            WorkGuiManager.AskPermition(shitter);
        });
    }

    private IEnumerator ShitterShiting(Shitter shitter)
    {
        var shitAmmountPerTick = shitter.ShitAmmount / 100f;
        var timeToWait = shitter.TimeShitting / 100f;
        StartCoroutine(MakeFartNoises(shitter.TimeShitting));
        for (int i = 0; i < 100; i++)
        {
            yield return new WaitForSeconds(timeToWait);
            GameManager.Instance.ShitAmmount += shitAmmountPerTick;
        }
        ShitterLeave(false);
    }

    private IEnumerator MakeFartNoises(float time)
    {
        var shortFartLenght = SoundManager.Instance.Sounds.Find(s => s.AudioId == AudioId.FartShort).AudioSource.clip.length;
        var mediumFartLenght = SoundManager.Instance.Sounds.Find(s => s.AudioId == AudioId.FartMedium).AudioSource.clip.length;
        var longFartLenght = SoundManager.Instance.Sounds.Find(s => s.AudioId == AudioId.FartLong).AudioSource.clip.length;

        var startTime = Time.time;
        float lastTimeShortFart = 0;
        float lastTimeMediumFart = 0;
        float lastTimeLongFart = 0;

        var random = new Random();

        while (Time.time - startTime < time)
        {
            float toWait = 0f;
            float elapsedTime = (Time.time - startTime);
            float secondsLeft = time - elapsedTime;

            if (secondsLeft > longFartLenght && (Time.time - lastTimeLongFart) > longFartLenght && random.NextDouble() < .3f)
            {
                SoundManager.Instance.PlayAudio(AudioId.FartLong);
                toWait = longFartLenght * 2;
                lastTimeLongFart = Time.time;
            }
            else if (secondsLeft > mediumFartLenght && (Time.time - lastTimeMediumFart) > mediumFartLenght && random.NextDouble() < .5f)
            {
                SoundManager.Instance.PlayAudio(AudioId.FartMedium);
                toWait = mediumFartLenght * 2;
                lastTimeMediumFart = Time.time;
            }
            else if (secondsLeft > shortFartLenght && (Time.time - lastTimeShortFart) > shortFartLenght && random.NextDouble() < .5f)
            {
                SoundManager.Instance.PlayAudio(AudioId.FartShort);
                toWait = shortFartLenght * 2;
                lastTimeShortFart = Time.time;
            }

            yield return new WaitForSeconds(toWait);
        }
    }

    public void ShitterLeave(bool denyied)
    {
        WorkGuiManager.ShitterLeave();
        CoroutineHelper.Instance.WaitForSecondsAndCall(.3f, UpdateQueue);
    }

    private void UpdateQueue()
    {
        if (!_dayGoing || _shitters.Count == 0)
        {
            EndDay();
            return;
        }

        ShitterArrive(_shitters.Dequeue());
    }

    #endregion

    #region callbacks

    private void OnShitterAccepted()
    {
        SoundManager.Instance.PlayAudio(AudioId.Granted);
        string message = _currentShitter.Accepted();

        var possibleMessagesForAccept = Shitter.PlayerReplyById[_currentShitter.LastDialogId];
        WorkGuiManager.ShowMessage(_currentShitter, Shitter.DialogByDialogId[possibleMessagesForAccept.AcceptDialog], () =>
        {
            WorkGuiManager.ShowMessage(_currentShitter, message, () =>
            {
                StartCoroutine(ShitterShiting(_currentShitter));
            });
        }, true, false);
    }

    private void OnShitterDenied()
    {
        SoundManager.Instance.PlayAudio(AudioId.Denyed);
        string message = _currentShitter.Denied();
        Action callback = () =>
        {
            ShitterLeave(true);
        };

        if (_currentShitter.SocialPosition == SocialPosition.Royalty)
        {
            callback = () =>
            {
                GameManager.Instance.EndGame(EndOptions.DenyRoialty);
                GameManager.Instance.LoadHouseScene();
            };
        }
        else if (_currentShitter.SocialPosition == SocialPosition.Cleric)
        {
            if (GameManager.Instance.CanDenyCleric)
            {
                GameManager.Instance.ClericDenyed++;
            }
            else
            {
                callback = () =>
                {
                    GameManager.Instance.EndGame(EndOptions.DenyCleric);
                    GameManager.Instance.LoadHouseScene();
                };
            }
        }

        var possibleMessagesForAccept = Shitter.PlayerReplyById[_currentShitter.LastDialogId];
        WorkGuiManager.ShowMessage(_currentShitter, Shitter.DialogByDialogId[possibleMessagesForAccept.DenyDialog], () =>
        {
            WorkGuiManager.ShowMessage(_currentShitter, message, callback, accepted: false);
        }, true, false);
    }

    #endregion

}
