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

    public Color ColorForPlayerMessage;

    public event Action OnAccept;
    public event Action OnDeny;

    #region Unity events

    void Start()
    {
        GameManager.Instance.OnShitAmmountChanges += UpdateShitCounter;
        CurrentShitterItem.FadeOut();
        UpdateShitCounter();
    }

    void OnDestroy()
    {
        GameManager.Instance.OnShitAmmountChanges -= UpdateShitCounter;
    }

    #endregion

    #region UI callbacks

    public void AcceptCallback()
    {
        SetButtons(false);
        if (OnAccept != null)
            OnAccept();
    }

    public void DenyCallback()
    {
        SetButtons(false);
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
    }

    #endregion

    #region UI

    public void AskPermition(Shitter shitter)
    {
        InnerPlayeMessage(shitter, shitter.GetMessageForPlayer(), () =>
        {
            SetButtons(true);
        });
    }

    public void UpdateShitCounter()
    {
        ShitCounter.fillAmount = GameManager.Instance.ShitAmmount / ScriptableObjectHolder.Instance.GameConfiguration.MaxShitAmmount;
    }

    public void ShowMessage(Shitter shitter, string message, Action callback, bool fromPlayer = false)
    {
        Color cached = TextMessage.color;
        if (fromPlayer)
        {
            TextMessage.color = ColorForPlayerMessage;
        }
        InnerPlayeMessage(shitter, message, () =>
        {
            if (fromPlayer)
                TextMessage.color = cached;

            if (callback != null)
                callback();
        });
    }

    private void InnerPlayeMessage(Shitter shitter, string message, Action callback)
    {
        TextMessage.text = string.Empty;
        StartCoroutine(CoroutineHelper.Instance.ShowText(message, (text) =>
        {
            TextMessage.text = text;
        }, callback));
    }

    private void SetButtons(bool enable)
    {
        AcceptButton.interactable = enable;
        DenyButton.interactable = enable;
    }
    
    #endregion
}
