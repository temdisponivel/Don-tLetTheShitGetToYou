using UnityEngine;
using System.Collections;
using DG.Tweening;

public class TitleGuiManager : MonoBehaviour
{
    public GameObject PanelCredits;

    private bool _creditsOpen;

    public void OnPlay()
    {
        GameManager.Instance.LoadHouseScene();
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
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
