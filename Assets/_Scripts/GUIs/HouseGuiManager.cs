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

    public void PlayMenssageOfTheDay(string message)
    {
        StartCoroutine(InnerPlayeMessage(message));
    }

    private IEnumerator InnerPlayeMessage(string message)
    {
        StringBuilder messageBuilder = new StringBuilder(message.Length);
        for (int i = 0; i < message.Length; i++)
        {
            messageBuilder.Append(message[i]);
            TextMessage.text = messageBuilder.ToString();
            yield return new WaitForSeconds(.1f);
        }
        
        yield return new WaitForSeconds(1f);
        GoToWorkButton.transform.DOScale(Vector3.one, .3f);
    }
}
