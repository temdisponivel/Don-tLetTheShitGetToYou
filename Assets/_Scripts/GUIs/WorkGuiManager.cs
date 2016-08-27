using System;
using UnityEngine;
using System.Collections;
using System.Text;
using DG.Tweening;
using UnityEngine.UI;

public class WorkGuiManager : MonoBehaviour
{
    public ShitterQueueItem CurrentShitterItem;
    public Text TextName;
    public Text TextMessage;
    public Button AcceptButton;
    public Button DenyButton;
    public Image ShitCounter;
    public Image ImageShitter;

    public event Action OnAccept;
    public event Action OnDeny;

    #region Unity events

    void Start()
    {
        GameManager.Instance.OnShitAmmountChanges += UpdateShitCounter;
        CurrentShitterItem.FadeOut();
    }

    void OnDestroy()
    {
        GameManager.Instance.OnShitAmmountChanges -= UpdateShitCounter;
    }

    #endregion

    #region UI callbacks

    public void AcceptCallback()
    {
        if (OnAccept != null)
            OnAccept();
    }

    public void DenyCallback()
    {
        if (OnDeny != null)
            OnDeny();
    }

    #endregion

    #region Shitters

    public void ShitterArrive(Shitter shitter, Action callback)
    {
        ImageShitter.sprite = shitter.SpriteShitter;
        DOTween.ToAlpha(() => ImageShitter.color, (color) =>
        {
            ImageShitter.color = color;
        }, 1f, 1f);
        ImageShitter.transform.DOScale(Vector3.one, .5f).OnComplete(() =>
        {
            if (callback != null)
                callback();
        });
        CurrentShitterItem.Setup(shitter);
        CurrentShitterItem.FadeIn();
        SetButtons(false);
    }

    public void ShitterLeave()
    {
        DOTween.ToAlpha(() => ImageShitter.color, (color) =>
        {
            ImageShitter.color = color;
        }, 0f, 1f);
        CurrentShitterItem.FadeOut();
        TextName.text = string.Empty;
        TextMessage.text = string.Empty;
        SetButtons(false);
    }

    #endregion

    #region UI

    public void AskPermition(Shitter shitter)
    {
        StartCoroutine(InnerPlayeMessage(shitter, shitter.GetMessageForPlayer(), () =>
        {
            SetButtons(true);
        }));
    }

    public void UpdateShitCounter()
    {
        ShitCounter.fillAmount = GameManager.Instance.ShitAmmount / ScriptableObjectHolder.Instance.GameConfiguration.MaxShitAmmount;
    }

    public void ShowMessage(Shitter shitter, string message, Action callback)
    {
        StartCoroutine(InnerPlayeMessage(shitter, message, callback));
    }

    private IEnumerator InnerPlayeMessage(Shitter shitter, string message, Action callback)
    {
        TextMessage.text = string.Empty;

        StartCoroutine(ShowName(shitter));

        StringBuilder messageBuilder = new StringBuilder(message.Length);
        for (int i = 0; i < message.Length; i++)
        {
            messageBuilder.Append(message[i]);
            TextMessage.text = messageBuilder.ToString();
            yield return new WaitForSeconds(.1f);
        }

        if (callback != null)
            callback();
    }

    private IEnumerator ShowName(Shitter shitter)
    {
        string shitterName = string.Format("{0} - {1}", shitter.Name, shitter.SocialPosition);

        if (TextName.text == shitterName)
            yield break;

        TextName.text = string.Empty;
        StringBuilder messageBuilder = new StringBuilder(shitterName.Length);
        for (int i = 0; i < shitterName.Length; i++)
        {
            messageBuilder.Append(shitterName[i]);
            TextName.text = messageBuilder.ToString();
            yield return new WaitForSeconds(.1f);
        }

    }

    private void SetButtons(bool enable)
    {
        AcceptButton.interactable = enable;
        DenyButton.interactable = enable;
    }

    private void SetCurrentShitterItem(Shitter shitter)
    {
        
    }

    #endregion
}
