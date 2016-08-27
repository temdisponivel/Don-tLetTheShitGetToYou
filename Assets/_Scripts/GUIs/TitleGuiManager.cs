using UnityEngine;
using System.Collections;

public class TitleGuiManager : MonoBehaviour
{
    public void OnPlay()
    {
        GameManager.Instance.LoadHouseScene();
    }

    public void OnQuit()
    {
        
    }

    public void Credits()
    {
        
    }
}
