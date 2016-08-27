using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorkLevelManager : MonoBehaviour
{
    public static WorkLevelManager Instance = null;
    public QueueManager QueueManager;
    public WorkGuiManager WorkGuiManager;
    private Queue<Shitter> _shitters;
    private bool _dayGoing = true;
    private Shitter _currentShitter = null;

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

    void EndDay()
    {
        GameManager.Instance.OnEndDay -= EndDay;
        GameManager.Instance.LoadHouseScene();
        _dayGoing = false;
    }

    public void ShitterArrive(Shitter shitter)
    {
        _currentShitter = shitter;
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

    private void OnShitterAccepted()
    {
        string message = _currentShitter.Accepted();
        QueueManager.RemovePeople(_currentShitter);
        WorkGuiManager.ShowMessage(_currentShitter, message, () =>
        {
            StartCoroutine(ShitterShiting(_currentShitter));
        });
    }

    private void OnShitterDenied()
    {
        string message = _currentShitter.Denied();
        QueueManager.RemovePeople(_currentShitter);
        WorkGuiManager.ShowMessage(_currentShitter, message, () =>
        {
            ShitterLeave(true);
        });
    }
}
