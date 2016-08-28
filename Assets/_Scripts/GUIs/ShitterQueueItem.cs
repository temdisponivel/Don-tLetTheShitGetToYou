using System.Collections;
using System.Text;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ShitterQueueItem : MonoBehaviour
{
    public Shitter Shitter;
    public Image ShitterImage;
    public Image ImageShitAmmount;
    public Image SocialPosition;
    public Text ShitterNameText;

    public void Setup(Shitter shitter)
    {
        Shitter = shitter;

        if (ShitterNameText != null)
        {
            StartCoroutine(SetName());
        }

        ShitterImage.sprite = shitter.SpriteShitter;

        var scaleValue = shitter.ShitAmmount / (ScriptableObjectHolder.Instance.GameConfiguration.MaxShitAmmount / (GameManager.Instance.TodaysShitters.Count * 1f));
        scaleValue = Mathf.Min(scaleValue, 1f);
        ImageShitAmmount.transform.localScale = new Vector3(scaleValue, scaleValue, 1f);

        SocialPosition.sprite = ScriptableObjectHolder.Instance.GameDatabase.SpriteBySocialPosition.Find(t => t.SocialPosition == shitter.SocialPosition).Sprite;
    }

    #region Fade

    public void FadeOut()
    {
        DOTween.ToAlpha(() => ShitterImage.color, (color) =>
        {
            ShitterImage.color = color;
        }, 0f, .5f);

        DOTween.ToAlpha(() => SocialPosition.color, (color) =>
        {
            SocialPosition.color = color;
        }, 0f, .5f);

        DOTween.ToAlpha(() => ImageShitAmmount.color, (color) =>
        {
            ImageShitAmmount.color = color;
        }, 0f, .5f);

        if (ShitterNameText != null)
        {
            DOTween.ToAlpha(() => ShitterNameText.color, (color) =>
            {
                ShitterNameText.color = color;
            }, 0f, .5f).OnComplete(() =>
            {
                ShitterNameText.text = string.Empty;
            });
        }
    }

    public void FadeIn()
    {
        DOTween.ToAlpha(() => ShitterImage.color, (color) =>
        {
            ShitterImage.color = color;
        }, 1f, .5f);

        DOTween.ToAlpha(() => SocialPosition.color, (color) =>
        {
            SocialPosition.color = color;
        }, 1f, .5f);

        DOTween.ToAlpha(() => ImageShitAmmount.color, (color) =>
        {
            ImageShitAmmount.color = color;
        }, 1f, .5f);

        if (ShitterNameText != null)
        {
            DOTween.ToAlpha(() => ShitterNameText.color, (color) =>
            {
                ShitterNameText.color = color;
            }, 1f, .5f);
        }
    }

    #endregion

    private IEnumerator SetName()
    {
        StringBuilder messageBuilder = new StringBuilder(Shitter.Name.Length);
        for (int i = 0; i < Shitter.Name.Length; i++)
        {
            messageBuilder.Append(Shitter.Name[i]);
            ShitterNameText.text = messageBuilder.ToString();
            yield return new WaitForSeconds(.1f);
        }

    }
}