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

    public void OnGoToWork()
    {
        GameManager.Instance.GoToWork();
    }

    public void ShowMenssageOfTheDay(string message)
    {
        StartCoroutine(InnerPlayeMessage(message, () =>
        {
            GoToWorkButton.transform.DOScale(Vector3.one, .3f);
        }));
    }

    public void ShowMessage(string message, Action callback)
    {
        StartCoroutine(InnerPlayeMessage(message, callback));
    }

    private IEnumerator InnerPlayeMessage(string message, Action callback)
    {
        StringBuilder messageBuilder = new StringBuilder(message.Length);
        for (int i = 0; i < message.Length; i++)
        {
            messageBuilder.Append(message[i]);
            TextMessage.text = messageBuilder.ToString();
            yield return new WaitForSeconds(.1f);
        }

        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        if (callback != null)
            callback();
    }
}