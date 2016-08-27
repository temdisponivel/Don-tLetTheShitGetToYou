using UnityEngine;
using System.Collections;

public class WorkLevelManager : MonoBehaviour
{
    public static WorkLevelManager Instance = null;

    public WorkGuiManager WorkGuiManager;

    void Awake()
    {
        Instance = this;
    }
    
}
