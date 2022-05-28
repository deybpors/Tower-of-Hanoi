using System.Collections;
using UnityEngine;

public class UIScale : UIAnimation
{
    public bool useCurrentScaleAsStart;
    public Vector3 start;
    public Vector3 end;
    private GameObject _thisObject;

    void Start()
    {
        _thisObject = gameObject;
    }

    void OnEnable()
    {
        try
        {
            Manager.instance.audioManager.PlaySfx(enableSound, true, true);
        }
        catch
        {
            //ignore
        }

        StopAllCoroutines();
        start = useCurrentScaleAsStart ? uiObject.localScale : start;
        uiObject.localScale = start;
        coroutine = StartCoroutine(Animate(uiObject, end, duration));
    }

    public IEnumerator Animate(RectTransform uiObject, Vector3 endScale, float time)
    {
        yield return new WaitForSeconds(delay);

        var timeElapsed = 0f;
        while (timeElapsed <= time)
        {
            timeElapsed += Time.deltaTime;
            uiObject.localScale = Vector3.Lerp(uiObject.localScale, endScale, timeElapsed / time);
            yield return null;
        }

        StopAllCoroutines();
        coroutine = null;
    }

    public IEnumerator DisableAnimation()
    {
        yield return new WaitForSeconds(delay);

        var timeElapsed = 0f;

        uiObject.localScale = end;
        while (timeElapsed <= duration)
        {
            timeElapsed += Time.deltaTime;
            uiObject.localScale = Vector3.Lerp(uiObject.localScale, start, timeElapsed / duration);
            yield return null;
        }

        coroutine = null;
        StopAllCoroutines();
        _thisObject.SetActive(false);
    }

    public override void Disable()
    {
        base.Disable();
        StopAllCoroutines();
        coroutine = StartCoroutine(DisableAnimation());
    }
}
