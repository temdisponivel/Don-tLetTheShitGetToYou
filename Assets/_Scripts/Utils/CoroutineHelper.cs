using System;
using UnityEngine;
using System.Collections;

public class CoroutineHelper : Singleton<CoroutineHelper>
{
    public void WaitForSecondsAndCall(float seconds, Action callback)
    {
        StartCoroutine(InnerWaitForSecondsAndCall(seconds, callback));
    }

    private IEnumerator InnerWaitForSecondsAndCall(float seconds, Action callback)
    {
        yield return new WaitForSeconds(seconds);
        if (callback != null)
            callback();
    }
}
