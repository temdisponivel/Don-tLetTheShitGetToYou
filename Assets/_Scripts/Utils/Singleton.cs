using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T _instance = null;

    private static object locker = new object();

    public static T Instance
    {
        get
        {
            lock (locker)
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<T>();

                    if (_instance == null)
                        _instance = new GameObject(typeof(T).Name + " SINGLETON").AddComponent<T>();
                }

                return _instance;
            }
        }
    }

    protected virtual void Awake()
    {
        GameObject.DontDestroyOnLoad(Instance);
    }
}
