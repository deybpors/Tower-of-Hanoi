
using System.Collections;
using UnityEngine;

public class ObjectScale : ObjectAnimation
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
        start = useCurrentPositionAsStart ? transformObject.localScale : start;
        transformObject.localScale = start;
        coroutine = StartCoroutine(Animate(transformObject, start, end, duration));
    }

    public IEnumerator Animate(Transform obj, Vector3 startPosition, Vector3 endScale, float time)
    {
        yield return new WaitForSeconds(delay);

        var timeElapsed = 0f;
        while (timeElapsed <= time)
        {
            timeElapsed += Time.deltaTime;
            obj.localScale = Vector3.Lerp(obj.localScale, endScale, timeElapsed / time);
            yield return null;
        }

        coroutine = null;
    }

    public IEnumerator Disable()
    {
        yield return new WaitForSeconds(delay);

        var timeElapsed = 0f;
        transformObject.localScale = end;
        while (timeElapsed <= duration)
        {
            timeElapsed += Time.deltaTime;
            transformObject.localScale = Vector3.Lerp(transformObject.localScale, start, timeElapsed / duration);
            yield return null;
        }

        _thisObject.SetActive(false);
    }
}
