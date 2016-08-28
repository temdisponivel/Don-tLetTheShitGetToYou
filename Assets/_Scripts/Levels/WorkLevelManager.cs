using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

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
        GameManager.Instance.OnEndDay += EndDay;
    }

    void Start()
    {
        WorkGuiManager.OnAccept += OnShitterAccepted;
        WorkGuiManager.OnDeny += OnShitterDenied;

        var shitters = GameManager.Instance.GetShittersForToday();
        _shitters = new Queue<Shitter>();

        for (int i = 0; i < shitters.Count; i++)
        {
            QueueManager.AddPeople(shitters[i]);
            _shitters.Enqueue(shitters[i]);
        }

        CoroutineHelper.Instance.WaitForSecondsAndCall(1f, UpdateQueue);
    }

    #endregion

    #region Game events

    void EndDay()
    {
        GameManager.Instance.OnEndDay -= EndDay;

        if (_dayGoing)
            GameManager.Instance.EndGame(EndOptions.ShitterInTheQueue);

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
        for (int i = 0; i < 100; i++)
        {
            yield return new WaitForSeconds(timeToWait);
            GameManager.Instance.ShitAmmount += shitAmmountPerTick;
        }
        ShitterLeave(false);
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
        string message = _currentShitter.Accepted();

        var possibleMessagesForAccept = ScriptableObjectHolder.Instance.GameDatabaseScriptableObject.PlayerAcceptReplies.Find(d => d.SocialPosition == _currentShitter.SocialPosition);
        var dialog = possibleMessagesForAccept.Dialogs[Random.Range(0, possibleMessagesForAccept.Dialogs.Count)];
        WorkGuiManager.ShowMessage(_currentShitter, Shitter.DialogByDialogId[dialog], () =>
        {
            WorkGuiManager.ShowMessage(_currentShitter, message, () =>
            {
                StartCoroutine(ShitterShiting(_currentShitter));
            });
        });
    }

    private void OnShitterDenied()
    {
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
                    GameManager.Instance.EndGame(EndOptions.DenyRoialty);
                };
            }
        }

        var possibleMessagesForDeny = ScriptableObjectHolder.Instance.GameDatabaseScriptableObject.PlayerDeniesReplies.Find(d => d.SocialPosition == _currentShitter.SocialPosition);
        var dialog = possibleMessagesForDeny.Dialogs[Random.Range(0, possibleMessagesForDeny.Dialogs.Count)];
        WorkGuiManager.ShowMessage(_currentShitter, Shitter.DialogByDialogId[dialog], () =>
        {
            WorkGuiManager.ShowMessage(_currentShitter, message, () =>
            {
                callback();
            });
        });
    }

    #endregion

}
