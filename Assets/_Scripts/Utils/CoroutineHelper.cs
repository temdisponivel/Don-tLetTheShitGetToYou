using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class CoroutineHelper : Singleton<CoroutineHelper>
{
    public event Action ButtonDown;

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

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (ButtonDown != null)
                ButtonDown();
        }
    }

    public IEnumerator ShowText(string text, Action<string> setter, Action callback, float intervalChars = .1f, float delayCallback = .5f, bool waitForClickToCallback = false, bool skipWithClick = true)
    {
        bool clicked = false;
        Action buttonDownCallback = () =>
        {
            clicked = true;
        };
        if (skipWithClick)
            ButtonDown += buttonDownCallback;

        StringBuilder messageBuilder = new StringBuilder(text.Length);
        for (int i = 0; i < text.Length; i++)
        {
            if (skipWithClick && clicked)
            {
                setter(text);
                break;
            }

            messageBuilder.Append(text[i]);
            setter(messageBuilder.ToString());
            yield return new WaitForSeconds(intervalChars);
        }

        if (skipWithClick)
            ButtonDown -= buttonDownCallback;

        if (waitForClickToCallback)
            yield return new WaitUntil(() => Input.GetMouseButtonUp(0));

        if (delayCallback > 0)
            yield return new WaitForSeconds(delayCallback);

        if (callback != null)
            callback();
    }
}
