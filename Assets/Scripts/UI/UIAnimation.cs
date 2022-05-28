using System.Collections;
using UnityEngine;

public class UIAnimation : MonoBehaviour
{
    public string enableSound;
    public string disableSound;
    public float duration;
    public float delay;
    
    [HideInInspector] public Coroutine coroutine;
    [HideInInspector] public RectTransform uiObject;
    [HideInInspector] public GameObject thisObject;

    void Awake()
    {
        uiObject = GetComponent<RectTransform>();
        thisObject = gameObject;
    }

    public void Enable()
    {
        if (thisObject == null)
        {
            thisObject = gameObject;
        }
        thisObject.SetActive(true);
    }

    public virtual void Disable()
    {
        try
        {
            Manager.instance.audioManager.PlaySfx(disableSound, true, true);
        }
        catch
        {
            //ignore
        }
    }

    public bool AnimationDone()
    {
        return coroutine == null;
    }
}
