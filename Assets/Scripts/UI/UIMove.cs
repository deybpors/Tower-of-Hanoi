using System;
using System.Collections;
using UnityEngine;

public class UIMove : UIAnimation
{
    public bool useCurrentPositionAsStart;
    public Vector3 start;
    public Vector3 end;
    private GameObject _thisObject;

    void Start()
    {
        _thisObject = gameObject;
    }

    void OnEnable()
    {
        StopAllCoroutines();
        start = useCurrentPositionAsStart ? uiObject.anchoredPosition : start;
        uiObject.anchoredPosition = start;
        coroutine = StartCoroutine(Animate(uiObject, end, duration));
    }


    public IEnumerator Animate(RectTransform uiObject, Vector3 endPosition, float time)
    {
        yield return new WaitForSeconds(delay);

        var timeElapsed = 0f;
        while (timeElapsed <= time)
        {
            timeElapsed += Time.deltaTime;
            uiObject.anchoredPosition = Vector3.Lerp(uiObject.anchoredPosition, endPosition, timeElapsed / time);
            yield return null;
        }

        StopAllCoroutines();
        coroutine = null;
    }

    public IEnumerator DisableAnimation()
    {
        uiObject.anchoredPosition = end;

        yield return new WaitForSeconds(delay);

        var timeElapsed = 0f;

        var time = duration * .25f;
        while (timeElapsed <= time)
        {
            timeElapsed += Time.deltaTime;
            uiObject.anchoredPosition = Vector3.Lerp(uiObject.anchoredPosition, start, timeElapsed / time);
            yield return null;
        }

        coroutine = null;
        StopAllCoroutines();
        _thisObject.SetActive(false);
    }

    public override void Disable()
    {
        StopAllCoroutines();
        coroutine = StartCoroutine(DisableAnimation());
    }
}
