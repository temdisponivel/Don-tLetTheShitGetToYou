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

    void Start()
    {
        if (GameManager.Instance.GameEnded)
        {
            GameManager.Instance.Reset();
            GoToWorkButton.GetComponentInChildren<Text>().text = "Restart";
        }
    }

    public void OnGoToWork()
    {
        if (GameManager.Instance.GameEnded)
        {
            GameManager.Instance.LoadTitleScene();
        }
        else
        {
            GameManager.Instance.GoToWork();
        }
    }

    public void ShowMessageOfTheDay(string message)
    {
        InnerPlayeMessage(message, () =>
        {
            GoToWorkButton.transform.DOScale(Vector3.one, .3f);
        });
    }

    public void ShowMessage(string message, Action callback)
    {
        InnerPlayeMessage(message, callback);
    }

    private void InnerPlayeMessage(string message, Action callback)
    {
        StartCoroutine(CoroutineHelper.Instance.ShowText(message, (text) =>
        {
            TextMessage.text = text;
        }, callback, waitForClickToCallback: true, playSound: true));
    }
}