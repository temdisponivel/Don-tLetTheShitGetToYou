using System;
using System.Collections;
using System.Text;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HouseGuiManager : MonoBehaviour
{
    public Button GoToWorkButton;
    public Text TextMessage;

    public event Action OnGoToWorkCallback;

    void Start()
    {
        if (GameManager.Instance.GameEnded)
        {
            GoToWorkButton.GetComponentInChildren<Text>().text = "Restart";
        }
    }

    public void OnGoToWork()
    {
        GoToWorkButton.transform.DOScale(Vector3.zero, .3f);

        if (OnGoToWorkCallback != null)
            OnGoToWorkCallback();
    }

    public void ShowMessageOfTheDay(string message, Action callback, bool hideDiary)
    {
        if (!hideDiary)
            message = message.Insert(0, "Journal: ");
        InnerPlayeMessage(message, () =>
        {
            if (callback != null)
                callback();
            GoToWorkButton.transform.DOScale(Vector3.one, .3f);
        });
    }

    public void ShowMessage(string message, Action callback, bool hideDiary)
    {
        if (!hideDiary)
            message = message.Insert(0, "Journal: ");
        InnerPlayeMessage(message, callback);
    }

    private void InnerPlayeMessage(string message, Action callback)
    {
        StartCoroutine(CoroutineHelper.Instance.ShowText(message, (text) =>
        {
            TextMessage.text = text;
        }, callback, waitForClickToCallback: true, playSound: true));
    }

    public void ChangeTextOfButton(string label)
    {
        GoToWorkButton.GetComponentInChildren<Text>().text = label;
    }
}