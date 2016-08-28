using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEditor;
using UnityEngine.UI;

public class TitleGuiManager : MonoBehaviour
{
    public GameObject PanelCredits;

    public Text TextMessage;

    private bool _creditsOpen;

    void Start()
    {
        SoundManager.Instance.StopAll();
        SoundManager.Instance.PlayAudio(AudioId.Ambiance);

        StartCoroutine(CoroutineHelper.Instance.ShowText(ScriptableObjectHolder.Instance.GameDatabase.TitleMessage, (message) =>
        {
            TextMessage.text = message;
        }, null, playSound: true));
    }

    public void OnPlay()
    {
        GameManager.Instance.LoadHouseScene();
        PlayClickSound();
    }

    public void Credits()
    {
        _creditsOpen = !_creditsOpen;

        if (_creditsOpen)
        {
            PanelCredits.transform.DOScale(Vector3.one, .5f);
        }
        else
        {
            PanelCredits.transform.DOScale(Vector3.zero, .5f);
        }
        PlayClickSound();
    }

    public void ToggleSound(bool value)
    {
        SoundManager.Instance.SoundEnable = value;
    }

    public void OnQuit()
    {
        Application.Quit();
        PlayClickSound();
    }

    private void PlayClickSound()
    {
    }
}
