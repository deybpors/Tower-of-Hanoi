using System.Collections;
using UnityEngine;

public class UIFade : UIAnimation
{
    public bool useCurrentFadeAsStart;
    public float start;
    public float end;
    private GameObject _thisObject;
    private CanvasGroup _canvasGroup;

    void Start()
    {
        _thisObject = gameObject;
    }

    void OnEnable()
    {
        StopAllCoroutines();

        if (_thisObject == null)
        {
            _thisObject = gameObject;
        }
        
        if (_canvasGroup == null)
        {
            _canvasGroup = _thisObject.AddComponent<CanvasGroup>();
        }

        start = useCurrentFadeAsStart ? _canvasGroup.alpha : start;
        _canvasGroup.alpha = start;
        coroutine = StartCoroutine(Animate(end, duration));
    }

    public IEnumerator Animate(float endPosition, float time)
    {
        yield return new WaitForSeconds(delay);

        var timeElapsed = 0f;
        while (timeElapsed <= time)
        {
            timeElapsed += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha, endPosition, timeElapsed / time);
            yield return null;
        }

        StopAllCoroutines();
        coroutine = null;
    }

    public IEnumerator DisableAnimation()
    {
        _canvasGroup.alpha = end;

        yield return new WaitForSeconds(delay);

        var timeElapsed = 0f;

        var time = duration * .25f;
        while (timeElapsed <= time)
        {
            timeElapsed += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha, start, timeElapsed / time);
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
